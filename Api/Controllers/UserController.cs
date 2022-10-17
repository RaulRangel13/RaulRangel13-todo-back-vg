using Domain.DTOs;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IIdentityService _identityService;

        public UserController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<UserCreateResponseDto>> Cadastrar(UserCreateRequestDto usuarioCadastro)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var resultado = await _identityService.CadastrarUsuario(usuarioCadastro);
            if (resultado.Sucesso)
                return Ok(resultado);
            else if (resultado.ErrosMessage.Count > 0)
                return BadRequest(resultado);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponseDto>> Login(UserLoginRequestDto usuarioLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var resultado = await _identityService.Login(usuarioLogin);
            if (resultado.Sucesso)
                return Ok(resultado);

            return Unauthorized(resultado);
        }

        [Authorize]
        [HttpPost("refresh-login")]
        public async Task<ActionResult<UserCreateResponseDto>> RefreshLogin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var usuarioId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return BadRequest();

            var resultado = await _identityService.LoginSemSenha(usuarioId);
            if (resultado.Sucesso)
                return Ok(resultado);

            return Unauthorized(resultado);
        }
    }
}
