using System.ComponentModel.DataAnnotations;
 
namespace API1.DatingApp.API.DTOs
{
    public class AuthUserDto
    {
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }
 
        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
    }
}
