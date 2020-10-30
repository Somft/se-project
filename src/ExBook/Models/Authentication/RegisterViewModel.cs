using System.ComponentModel.DataAnnotations;

namespace ExBook.Models.Authentication
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        public string Login { get; set; } = "";

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; } = "";

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string PasswordConfirmation { get; set; } = "";

        public string? Message { get; set; } = null;
    }
}
