using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserCreateResponseDto
    {
        public bool Sucesso { get; private set; }
        public List<string> ErrosMessage { get; private set; }

        public UserCreateResponseDto() =>
            ErrosMessage = new List<string>();

        public UserCreateResponseDto(bool sucesso = true) : this() =>
            Sucesso = sucesso;

        public void AdicionarErros(IEnumerable<string> erros) =>
            ErrosMessage.AddRange(erros);
    }
}
