using ExBook.Data;

using System.Collections.Generic;

namespace ExBook.Models.BookShelf
{
    public class BookShelf
    {
        public List<BookShelfBook> Books { get; internal set; }
    }
}