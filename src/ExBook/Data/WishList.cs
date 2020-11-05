using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBook.Data
{
    [Table("wish_list")]
    public partial class WishList
    {
        public WishList()
        {
            WishListBook = new HashSet<WishListBook>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("WishList")]
        public virtual User User { get; set; }
        [InverseProperty("WishList")]
        public virtual ICollection<WishListBook> WishListBook { get; set; }
    }
}
