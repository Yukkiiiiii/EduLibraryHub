using System.ComponentModel.DataAnnotations;

namespace EduLibraryHub.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User"; // Default role

        // Add other properties like email, first name, last name, etc.
    }
}