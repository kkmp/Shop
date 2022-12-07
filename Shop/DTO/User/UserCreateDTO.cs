using System.ComponentModel.DataAnnotations;

namespace Shop.DTO.User
{
    public class UserCreateDTO
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        [RegularExpression("^[0-9A-Za-ząęłńśćźżóĄĘŁŃŚĆŹŻÓ_-]{3,25}$", ErrorMessage = "Incorrect name")]
        public string? UserName { get; set; }

        [Required]
        [MaxLength(25)]
        [MinLength(8)]
        [RegularExpression("^[0-9A-Za-z_~!'?'@#'$'%'^'&'*''+''('')''{''}'><'.''|'',':;-]{6,25}$", ErrorMessage = "Incorrect password")]
        public string? Password1 { get; set; }

        [Required]
        [MaxLength(25)]
        [MinLength(8)]
        [Compare("Password1", ErrorMessage = "Passwords must be the same")]
        [RegularExpression("^[0-9A-Za-z_~!'?'@#'$'%'^'&'*''+''('')''{''}'><'.''|'',':;-]{6,25}$", ErrorMessage = "Incorrect password")]
        public string? Password2 { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(254)]
        [RegularExpression("[A-Z0-9a-z'.'_-]+@[A-Za-z0-9'.'-]+.[A-Za-z]{2,254}", ErrorMessage = "Incorrect email")]
        public string? Email { get; set; }
    }
}
