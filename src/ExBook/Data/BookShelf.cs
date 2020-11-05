using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBook.Data
{
    [Table("book_shelf")]
    public partial class BookShelf
    {
        public BookShelf()
        {
            BookShelfBook = new HashSet<BookShelfBook>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("BookShelf")]
        public virtual User User { get; set; }
        [InverseProperty("BookShelf")]
        public virtual ICollection<BookShelfBook> BookShelfBook { get; set; }
    }
}
