public class Book
{
    // Properties of the book
    public string Title { get; set; }
    public string Author { get; set; }
    public int YearOfPublishing { get; set; }
    public string Genre { get; set; }

    public Book(string title, string author, string isbn, int yearofpublishing, string genre)
    {
        Title = title;
        Author = author;
        YearOfPublishing = yearofpublishing;
        Genre = genre;
    }

    public void DisplayBookDetails()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Publication Year: {YearOfPublishing}");
        Console.WriteLine($"Genre: {Genre}");
    }
}

