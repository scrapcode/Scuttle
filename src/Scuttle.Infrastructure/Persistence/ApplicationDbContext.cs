using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Scuttle.Domain.Entities;

namespace Scuttle.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Corner> Corners { get; set; }
    public DbSet<Post> Posts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsRequired();
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(e => e.PasswordHash)
                .IsRequired();

            entity.HasIndex(e => e.Username)
                .IsUnique();
            entity.HasIndex(e => e.Email)
                .IsUnique();
        });

        // Corner
        modelBuilder.Entity<Corner>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(e => e.UrlSlug)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(e => e.Description)
                  .HasMaxLength(500);

            entity.HasIndex(e => e.UrlSlug)
                  .IsUnique();

            // Foreign key relationship with User (Creator)
            entity.HasOne(e => e.Creator)
                  .WithMany()
                  .HasForeignKey(e => e.CreatorId)
                  .OnDelete(DeleteBehavior.Restrict); // Don't delete corner if user is deleted
        });

        // Post
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                  .HasMaxLength(300)
                  .IsRequired();

            entity.Property(e => e.Content)
                  .HasMaxLength(10000)
                  .IsRequired();

            entity.HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Corner)
                .WithMany()
                .HasForeignKey(e => e.CornerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
