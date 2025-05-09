﻿using System.ComponentModel.DataAnnotations;

namespace EduLibraryHub.Data.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Заглавие")]
        public string? Title { get; set; }
        [Display(Name = "Автор")]
        public string? Author { get; set; }
        [Display(Name = "Година")]
        public string? ReleaseYear { get; set; }
        [Display(Name = "Том")]
        public string? Tome { get; set; }
        [Display(Name = "Наличност")]
        public string Inventory { get; set; }
        [Display(Name = "Genre")]
        public virtual Genre? Genre { get; set; }
        public int? GenreId { get; set; }
        public virtual List<Tag>? Tags { get; set; } = new List<Tag>();
        public virtual List<Review>? Reviews { get; set; }
        public virtual List<BorrowRecord>? BorrowRecords { get; set; }
    }
}
