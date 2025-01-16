using EduLibraryHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace OnlineLibrary.Models
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext() : base("LibraryDbConnection") { }

        public DbSet<User> Users { get; set; }
    }
}
