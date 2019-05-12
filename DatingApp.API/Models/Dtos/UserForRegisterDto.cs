using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Models.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password 4-8 chars")]
        public string Password { get; set; }
    }
}