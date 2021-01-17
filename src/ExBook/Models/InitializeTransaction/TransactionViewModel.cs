using ExBook.Data;

using System.Collections.Generic;


namespace ExBook.Models.Transaction
{
    public class TransactionViewModel
    {
        public Data.Transaction Transaction { get; set; } = null!;
        public BookShelfBook initialBook { get; set; } = null!;
        public ICollection<BookShelfBook> InitiatorBooks { get; set; } = null!;
        public ICollection<BookShelfBook> RecipientBooks { get; set; } = null!;

    }
}
