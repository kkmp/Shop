using System.ComponentModel.DataAnnotations;

namespace Shop.DTO.User
{
    public class UserLoginDTO
    {
        [Required]
        public string? Password { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}
