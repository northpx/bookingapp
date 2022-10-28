using System.ComponentModel.DataAnnotations;
 
namespace API1.DatingApp.API.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
 
        [Required]
        [MaxLength(256)]
        public string Username { get; set; }
 
        [MaxLength(256)]
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}
