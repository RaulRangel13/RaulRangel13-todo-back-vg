using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.DTOs;
using Domain.Interfaces.Services;
using Identity.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(SignInManager<IdentityUser> signInManager,
                               UserManager<IdentityUser> userManager,
                               IOptions<JwtOptions> jwtOptions)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<UserCreateResponseDto> CadastrarUsuario(UserCreateRequestDto usuarioCadastro)
        {
            var identityUser = new IdentityUser
            {
                UserName = usuarioCadastro.Email,
                Email = usuarioCadastro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, usuarioCadastro.Password);
            if (result.Succeeded)
                await _userManager.SetLockoutEnabledAsync(identityUser, false);

            var credentials = await GerarCredenciais(identityUser.Email);

            var usuarioCadastroResponse = new UserCreateResponseDto(result.Succeeded, credentials.AccessToken);
            if (!result.Succeeded && result.Errors.Count() > 0)
                usuarioCadastroResponse.AdicionarErros(result.Errors.Select(r => r.Description));

            return usuarioCadastroResponse;
        }

        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto usuarioLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Password, false, true);
            if (result.Succeeded)
                return await GerarCredenciais(usuarioLogin.Email);

            var usuarioLoginResponse = new UserLoginResponseDto();
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    usuarioLoginResponse.AdicionarErro("Essa conta está bloqueada");
                else if (result.IsNotAllowed)
                    usuarioLoginResponse.AdicionarErro("Essa conta não tem permissão para fazer login");
                else if (result.RequiresTwoFactor)
                    usuarioLoginResponse.AdicionarErro("É necessário confirmar o login no seu segundo fator de autenticação");
                else
                    usuarioLoginResponse.AdicionarErro("Usuário ou senha estão incorretos");
            }

            return usuarioLoginResponse;
        }

        public async Task<UserLoginResponseDto> LoginSemSenha(string usuarioId)
        {
            var usuarioLoginResponse = new UserLoginResponseDto();
            var usuario = await _userManager.FindByIdAsync(usuarioId);
            
            if (await _userManager.IsLockedOutAsync(usuario))
                usuarioLoginResponse.AdicionarErro("Essa conta está bloqueada");
            else if (!await _userManager.IsEmailConfirmedAsync(usuario))
                usuarioLoginResponse.AdicionarErro("Essa conta precisa confirmar seu e-mail antes de realizar o login");
            
            if (usuarioLoginResponse.Sucesso)
                return await GerarCredenciais(usuario.Email);

            return usuarioLoginResponse;
        }

        private async Task<UserLoginResponseDto> GerarCredenciais(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var accessTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: true);
            var refreshTokenClaims = await ObterClaims(user, adicionarClaimsUsuario: false);

            var dataExpiracaoAccessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);
            var dataExpiracaoRefreshToken = DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpiration);

            var accessToken = GerarToken(accessTokenClaims, dataExpiracaoAccessToken);
            var refreshToken = GerarToken(refreshTokenClaims, dataExpiracaoRefreshToken);

            return new UserLoginResponseDto
            (
                sucesso: true,
                accessToken: accessToken,
                refreshToken: refreshToken,
                email: user.Email,
                userName: user.UserName
            );
        }

        private string GerarToken(IEnumerable<Claim> claims, DateTime dataExpiracao)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: dataExpiracao,
                signingCredentials: _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<IList<Claim>> ObterClaims(IdentityUser user, bool adicionarClaimsUsuario)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            if (adicionarClaimsUsuario)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                claims.AddRange(userClaims);

                foreach (var role in roles)
                    claims.Add(new Claim("role", role));
            }

            return claims;
        }
    }
}