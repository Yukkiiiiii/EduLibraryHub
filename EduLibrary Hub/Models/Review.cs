using System.ComponentModel.DataAnnotations;

namespace EduLibraryHub.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; } // ������ ��� �����

        [Required]
        [MaxLength(500)]
        public string Content { get; set; } // ����� �� ���������

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // ������ �� 1 �� 5

        [Required]
        public string Username { get; set; } // ����������, ����� � ������� ���������

        public DateTime DatePosted { get; set; } = DateTime.Now; // ���� �� �����������
    }
}
