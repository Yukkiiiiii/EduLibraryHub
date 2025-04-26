using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EduLibraryHub.Data.Entities;
using EduLibraryHub.Models;

namespace EduLibraryHub.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = default!;

    public DbSet<Review> Reviews { get; set; } = default!;

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<UserViewModel> UserViewModels { get; set; } = default!;

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

        builder.Entity<Book>()
            .HasMany(b => b.Tags)
            .WithMany(t => t.Books)
            .UsingEntity(j => j.ToTable("BookTags"));
    }
}
