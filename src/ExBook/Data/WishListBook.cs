using System;
using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class WishListBook
    {
        public Guid Id { get; set; }
        public Guid? WishListId { get; set; }
        public Guid? BookId { get; set; }

        public virtual Book Book { get; set; }
        public virtual WishList WishList { get; set; }
    }
}
