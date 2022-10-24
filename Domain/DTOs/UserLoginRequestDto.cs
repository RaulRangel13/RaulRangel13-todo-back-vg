using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.DTOs
{
    public class UserLoginRequestDto
    {
        [Required(ErrorMessage = "O campo {0} � obrigat�rio")]
        [EmailAddress(ErrorMessage = "O campo {0} � inv�lido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} � obrigat�rio")]
        public string Password { get; set; }
    }
}