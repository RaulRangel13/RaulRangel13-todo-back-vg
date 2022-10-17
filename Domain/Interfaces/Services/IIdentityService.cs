
using Domain.DTOs;

namespace Domain.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<UserCreateResponseDto> CadastrarUsuario(UserCreateRequestDto usuarioCadastro);
        Task<UserLoginResponseDto> Login(UserLoginRequestDto usuarioLogin);
        Task<UserLoginResponseDto> LoginSemSenha(string usuarioId);
    }
}

