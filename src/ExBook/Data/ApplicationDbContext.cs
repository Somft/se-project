using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookShelf> BookShelves { get; set; }
        public virtual DbSet<BookShelfBook> BookShelfBooks { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }
        public virtual DbSet<WishListBook> WishListBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("book");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Author).HasColumnName("author");

                entity.Property(e => e.CoverUrl).HasColumnName("cover_url");

                entity.Property(e => e.Created)
                    .HasColumnType("date")
                    .HasColumnName("created");

                entity.Property(e => e.Isbn).HasColumnName("isbn");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<BookShelf>(entity =>
            {
                entity.ToTable("book_shelf");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookShelves)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("book_shelf_fk");
            });

            modelBuilder.Entity<BookShelfBook>(entity =>
            {
                entity.ToTable("book_shelf_book");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.BookShelfId).HasColumnName("book_shelf_id");

                entity.Property(e => e.Photo).HasColumnName("photo");

                entity.Property(e => e.IsLocked)
                    .IsRequired()
                    .HasDefaultValue(false)
                    .HasColumnName("is_locked");

                entity.Property(e => e.IsRemoved)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnName("is_removed");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookShelfBooks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("book_shelf_book_fk");

                entity.HasOne(d => d.BookShelf)
                    .WithMany(p => p.BookShelfBooks)
                    .HasForeignKey(d => d.BookShelfId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("book_shelf_book_fk_1");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("rating");

                entity.HasIndex(e => e.TransactionId, "rating_un")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Comment).HasColumnName("comment");

                entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasConversion<string>();

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("rating_fk");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("subject");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transaction");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.InitiatorId)
                    .IsRequired()
                    .HasColumnName("recipient_id");

                entity.Property(e => e.RecipientId)
                    .IsRequired()
                    .HasColumnName("initiator_id");

                entity.HasOne(d => d.Initiator)
                    .WithMany(p => p.InitiatedTransactions)
                    .HasForeignKey(d => d.InitiatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.ReceivedTransactions)
                    .HasForeignKey(d => d.RecipientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.IsEmailConfirmed)
                    .HasColumnName("is_email_confirmed");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasColumnName("role");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("surname");

                entity.Property(e => e.IsEmailAuthenticationEnabled)
                    .IsRequired()
                    .HasColumnName("is_email_authentication_enabled");

                entity.Property(e => e.ContactNumber)
                    .HasColumnName("contact_number");

                entity.Property(e => e.Address)
                    .HasColumnName("address");

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code");

                entity.Property(e => e.City)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasColumnName("country");
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.ToTable("wish_list");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WishLists)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("wish_list_fk");
            });

            modelBuilder.Entity<WishListBook>(entity =>
            {
                entity.ToTable("wish_list_book");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.WishListId).HasColumnName("wish_list_id");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.WishListBooks)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("wish_list_book_fk_1");

                entity.HasOne(d => d.WishList)
                    .WithMany(p => p.WishListBooks)
                    .HasForeignKey(d => d.WishListId)
                    .HasConstraintName("wish_list_book_fk");
            });

            modelBuilder.Entity<Book>()
                    .HasMany(x => x.Subjects)
                    .WithMany(x => x.Books)
                    .UsingEntity<Dictionary<string, object>>(
                        "book_subject",
                        x => x.HasOne<Subject>().WithMany().HasForeignKey("subject_id"),
                        x => x.HasOne<Book>().WithMany().HasForeignKey("book_id"));

            modelBuilder.Entity<Transaction>()
                    .HasMany(x => x.InitiatorBooks)
                    .WithMany(x => x.InitiatorTransactions)
                    .UsingEntity<Dictionary<string, object>>(
                        "transaction_initiator_book",
                        x => x.HasOne<BookShelfBook>().WithMany().HasForeignKey("book_shelf_book_id"),
                        x => x.HasOne<Transaction>().WithMany().HasForeignKey("transaction_id").OnDelete(DeleteBehavior.ClientCascade));

            modelBuilder.Entity<Transaction>()
                    .HasMany(x => x.RecipientBooks)
                    .WithMany(x => x.RecipientTransactions)
                    .UsingEntity<Dictionary<string, object>>(
                        "transaction_recipient_book",
                        x => x.HasOne<BookShelfBook>().WithMany().HasForeignKey("book_shelf_book_id"),
                        x => x.HasOne<Transaction>().WithMany().HasForeignKey("transaction_id").OnDelete(DeleteBehavior.ClientCascade));

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
