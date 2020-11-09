using System;
using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class Book
    {
        public Book()
        {
            this.BookShelfBooks = new HashSet<BookShelfBook>();
            this.WishListBooks = new HashSet<WishListBook>();
        }

        public Guid Id { get; set; }
        public string Author { get; set; }
        public DateTime? Created { get; set; }
        public string Isbn { get; set; }
        public string Name { get; set; }
        public string CoverUrl { get; set; }

        public virtual ICollection<BookShelfBook> BookShelfBooks { get; set; }
        public virtual ICollection<WishListBook> WishListBooks { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
