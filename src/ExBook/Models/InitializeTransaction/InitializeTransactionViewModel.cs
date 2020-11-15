using ExBook.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ExBook.Models.InitializeTransaction
{
    public class InitializeTransactionViewModel
    {
      
        public Data.Transaction transaction  { get; set; }
        public BookShelfBook initialBook { get; set; }
        public ICollection<BookShelfBook> initiatorBooks { get; set; }
        public ICollection<BookShelfBook>recipientBooks { get; set; }

    }
}
