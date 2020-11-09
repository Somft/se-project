using System;

#nullable disable

namespace ExBook.Data
{
    public partial class BookShelfBook
    {
        public Guid Id { get; set; }
        public Guid? Photo { get; set; }
        public Guid BookId { get; set; }
        public Guid BookShelfId { get; set; }

        public virtual Book Book { get; set; }
        public virtual BookShelf BookShelf { get; set; }
    }
}
