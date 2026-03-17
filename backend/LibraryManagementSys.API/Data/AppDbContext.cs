using LibraryManagementSys.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LibraryManagementSys.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → BorrowRecords (One to Many)
            modelBuilder.Entity<BorrowRecord>()
                .HasOne(b => b.User)
                .WithMany(u => u.BorrowRecords)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Book → BorrowRecords (One to Many)
            modelBuilder.Entity<BorrowRecord>()
                .HasOne(b => b.Book)
                .WithMany(bk => bk.BorrowRecords)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Admin User
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = 1,
                Name = "Admin",
                Email = "admin@library.com",
                Password = "$2a$11$fWsSOgldFQZh/wbv2zMuZOX4c0gSKCY0F/qopN8tPb2igbbQDI7Pa",
                Role = "Admin",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)

            });
        }
    }
}
