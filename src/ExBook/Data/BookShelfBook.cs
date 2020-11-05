using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBook.Data
{
    [Table("book_shelf_book")]
    public partial class BookShelfBook
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("photo")]
        public Guid? Photo { get; set; }
        [Column("book_id")]
        public Guid BookId { get; set; }
        [Column("book_shelf_id")]
        public Guid BookShelfId { get; set; }

        [ForeignKey(nameof(BookId))]
        [InverseProperty("BookShelfBook")]
        public virtual Book Book { get; set; }
        [ForeignKey(nameof(BookShelfId))]
        [InverseProperty("BookShelfBook")]
        public virtual BookShelf BookShelf { get; set; }
    }
}
