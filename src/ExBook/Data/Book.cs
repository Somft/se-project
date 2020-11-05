using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBook.Data
{
    [Table("book")]
    public partial class Book
    {
        public Book()
        {
            BookShelfBook = new HashSet<BookShelfBook>();
            WishListBook = new HashSet<WishListBook>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("author")]
        public string Author { get; set; }
        [Column("created", TypeName = "date")]
        public DateTime? Created { get; set; }
        [Column("isbn")]
        public string Isbn { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }

        [InverseProperty("Book")]
        public virtual ICollection<BookShelfBook> BookShelfBook { get; set; }
        [InverseProperty("Book")]
        public virtual ICollection<WishListBook> WishListBook { get; set; }
    }
}
