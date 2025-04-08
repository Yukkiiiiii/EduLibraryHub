using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class LibraryDbContext : IdentityDbContext<User>
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; } // Add Book Management

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Default Admin User
        var admin = new User
        {
            Id = "1",
            UserName = "admin@library.com",
            NormalizedUserName = "ADMIN@LIBRARY.COM",
            Email = "admin@library.com",
            NormalizedEmail = "ADMIN@LIBRARY.COM",
            EmailConfirmed = true,
            Name = "Admin",
            Role = "Admin"
        };

        var hasher = new PasswordHasher<User>();
        admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

        modelBuilder.Entity<User>().HasData(admin);
    }
}
