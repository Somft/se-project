using System;
using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class BookShelf
    {
        public BookShelf()
        {
            BookShelfBooks = new HashSet<BookShelfBook>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<BookShelfBook> BookShelfBooks { get; set; }
    }
}
