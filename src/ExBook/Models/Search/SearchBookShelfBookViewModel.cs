using ExBook.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Models.Search
{
    public class SearchBookShelfBookViewModel
    {
        public List<BookShelfBook> BookShelfBooks { get; set; }
        public string ? FilterTitle { get; set; }
        public string ? FilterLogin { get; set; }
    }
}
