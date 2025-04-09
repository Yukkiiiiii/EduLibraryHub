using Microsoft.EntityFrameworkCore;
using EduLibraryHub.Models;

namespace EduLibraryHub.database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

