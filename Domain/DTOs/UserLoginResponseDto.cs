using System.Text.Json.Serialization;

namespace Domain.DTOs
{
    public class UserLoginResponseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Sucesso  => Erros.Count == 0;
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AccessToken { get; private set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefreshToken { get; private set; }
        
        public List<string> Erros { get; private set; }

        public UserLoginResponseDto() =>
            Erros = new List<string>();

        public UserLoginResponseDto(bool sucesso, string accessToken, string refreshToken, string email, string userName) : this()
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Email = email;
            UserName = userName;
        }

        public void AdicionarErro(string erro) =>
            Erros.Add(erro);

        public void AdicionarErros(IEnumerable<string> erros) =>
            Erros.AddRange(erros);
    }
}