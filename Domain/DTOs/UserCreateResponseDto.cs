using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserCreateResponseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public bool Sucesso { get; private set; }
        public List<string> ErrosMessage { get; private set; }

        public UserCreateResponseDto() =>
            ErrosMessage = new List<string>();

        public UserCreateResponseDto(bool sucesso = true, string token = null) : this()
        {
            Sucesso = sucesso;
            AccessToken = token;
        }


        public void AdicionarErros(IEnumerable<string> erros) =>
            ErrosMessage.AddRange(erros);
    }
}
