namespace EduLibraryHub.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int ReleaseYear { get; set; }
        public string Tome { get; set; }
        public int Inventory { get; set; }

        public Book() { }

        public Book(string title, string author, string isbn, int releaseyear, string tome, int inventory)
        {
            Title = title;
            Author = author;
            ReleaseYear = releaseyear;
            Tome = tome;
            Inventory = inventory;
        }

        public void DisplayBookDetails()
        {
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Author: {Author}");
            Console.WriteLine($"Publication Year: {ReleaseYear}");
            Console.WriteLine($"Tome: {Tome}");
            Console.WriteLine($"Inventory: {Inventory}");
        }
    }
}


