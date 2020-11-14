using System;
using System.Collections.Generic;
using System.Linq;
using ExBook.Data;
using System.Threading.Tasks;

namespace ExBook.Models.InitializeTransaction
{
    public class InitializeTransactionViewModel
    {
        //current user bookshelf
        //bookshelf of a user receiving transaction
        public List<BookShelfBook> CurrentUserBooks  { get; set; }

    }
}
