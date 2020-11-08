using System;
using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class WishList
    {
        public WishList()
        {
            WishListBooks = new HashSet<WishListBook>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<WishListBook> WishListBooks { get; set; }
    }
}
