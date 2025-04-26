using System.ComponentModel.DataAnnotations;

namespace EduLibraryHub.Data.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public virtual List<Book>? Books { get; set; }
    }
}
