using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBook.Data
{
    [Table("wish_list_book")]
    public partial class WishListBook
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("wish_list_id")]
        public Guid? WishListId { get; set; }
        [Column("book_id")]
        public Guid? BookId { get; set; }

        [ForeignKey(nameof(BookId))]
        [InverseProperty("WishListBook")]
        public virtual Book Book { get; set; }
        [ForeignKey(nameof(WishListId))]
        [InverseProperty("WishListBook")]
        public virtual WishList WishList { get; set; }
    }
}
