using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Dtos.User
{
    public class LoginDto
    {
        [Required]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
