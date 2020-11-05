using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExBook.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Book { get; set; } = null!;
        public virtual DbSet<BookShelf> BookShelf { get; set; } = null!;
        public virtual DbSet<BookShelfBook> BookShelfBook { get; set; } = null!;
        public virtual DbSet<Rating> Rating { get; set; } = null!;
        public virtual DbSet<Subject> Subject { get; set; } = null!;
        public virtual DbSet<Transaction> Transaction { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<WishList> WishList { get; set; } = null!;
        public virtual DbSet<WishListBook> WishListBook { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<BookShelf>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookShelf)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("book_shelf_fk");
            });

            modelBuilder.Entity<BookShelfBook>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookShelfBook)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("book_shelf_book_fk");

                entity.HasOne(d => d.BookShelf)
                    .WithMany(p => p.BookShelfBook)
                    .HasForeignKey(d => d.BookShelfId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("book_shelf_book_fk_1");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasIndex(e => e.TransactionId)
                    .HasName("rating_un")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Transaction)
                    .WithOne(p => p.Rating)
                    .HasForeignKey<Rating>(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rating_fk");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("wish_list_fk");
            });

            modelBuilder.Entity<WishListBook>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.WishListBook)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("wish_list_book_fk_1");

                entity.HasOne(d => d.WishList)
                    .WithMany(p => p.WishListBook)
                    .HasForeignKey(d => d.WishListId)
                    .HasConstraintName("wish_list_book_fk");
            });

        }
    }
}
