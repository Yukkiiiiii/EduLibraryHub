using System.ComponentModel.DataAnnotations;

namespace EduLibraryHub.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Задължително потребителско име")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Задължителен имейл")]
        [EmailAddress(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Задължителна парола")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Потвърдете паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}