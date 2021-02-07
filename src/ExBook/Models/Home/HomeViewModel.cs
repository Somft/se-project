using ExBook.Data;

using System.Collections.Generic;


namespace ExBook.Models.Home
{
    public class HomeViewModel
    {
        public ICollection<Book> LatestBooks { get; set; } = null!;
    }
}
