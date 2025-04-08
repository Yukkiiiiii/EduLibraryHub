namespace EduLibraryHub.database
{
    using Microsoft.EntityFrameworkCore;
    using EduLibraryHub.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Book> Books { get; set; }
    }
}
