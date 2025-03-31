using System.ComponentModel.DataAnnotations;

namespace EduLibraryHub.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; } // Връзка към книга

        [Required]
        [MaxLength(500)]
        public string Content { get; set; } // Текст на коментара

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // Оценка от 1 до 5

        [Required]
        public string Username { get; set; } // Потребител, който е оставил коментара

        public DateTime DatePosted { get; set; } = DateTime.Now; // Дата на публикуване
    }
}
