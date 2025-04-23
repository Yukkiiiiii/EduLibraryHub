using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EduLibraryHub.Data.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; } // Връзка към книга
        [Display(Name ="Книга")]
        public virtual Book? Book { get; set; }

        [Required]
        [MaxLength(500)]
        [Display(Name ="Отзив")]
        public string Content { get; set; } // Текст на коментара

        [Required]
        [Range(1, 5)]
        [Display(Name="Рейтинг")]
        public int Rating { get; set; } // Оценка от 1 до 5
        
        public string? UserId { get; set; }
        [Display(Name ="Потребител")]
        public virtual IdentityUser? User { get; set; }
        [Display(Name ="Добавен")]
        public DateTime? DatePosted { get; set; }

    }
}
