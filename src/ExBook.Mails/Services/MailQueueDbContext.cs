using Microsoft.EntityFrameworkCore;

namespace ExBook.Mails.Services
{
    public class MailQueueDbContext : DbContext
    {
        public MailQueueDbContext(DbContextOptions<MailQueueDbContext> options)
        : base(options)
        {
        }

        public DbSet<Mail> Mails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mail>(entity =>
            {
                entity.ToTable("mail");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.To)
                    .IsRequired()
                    .HasDefaultValue("")
                    .HasColumnName("to");

                entity.Property(e => e.Subject)
                  .IsRequired()
                  .HasDefaultValue("")
                  .HasColumnName("subject");

                entity.Property(e => e.Content)
                  .IsRequired()
                  .HasDefaultValue("")
                  .HasColumnName("content");

                entity.Property(e => e.Error)
                  .HasColumnName("error");

                entity.Property(e => e.Success)
                  .IsRequired()
                  .HasDefaultValue(false)
                  .HasColumnName("success");
            });
        }
    }
}
