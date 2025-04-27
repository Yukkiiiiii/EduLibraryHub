namespace EduLibraryHub.Data.Entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

    }
}
