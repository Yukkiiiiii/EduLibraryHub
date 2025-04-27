namespace EduLibraryHub.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ReleaseYear { get; set; }
        public string Tome { get; set; }
        public string Inventory { get; set; }
        public string Genre { get; set; }
        public List<string> Tags { get; set; }
        public int ReviewsCount { get; set; }
    }
}
