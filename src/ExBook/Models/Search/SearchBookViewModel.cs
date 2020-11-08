using ExBook.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Models.Search
{
    public class SearchBookViewModel
    {
        public List<Book> Books { get; set; }
        public string? FilterTitle { get; set; }
        public string? FilterAuthor { get; set; }
        public bool FilterAvailable { get; set; }

        public Guid UserId { get; set; }

        
    }
}
