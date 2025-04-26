using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EduLibraryHub.Data.Entities;

namespace EduLibraryHub.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<EduLibraryHub.Data.Entities.Book> Book { get; set; } = default!;

    public DbSet<EduLibraryHub.Data.Entities.Review> Review { get; set; } = default!;

    public DbSet<EduLibraryHub.Models.UserViewModel> UserViewModel { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Review>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Reviews)
                .HasForeignKey(r => r.BookId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Review>()
            .HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
