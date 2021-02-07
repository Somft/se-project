using ExBook.Data;

using System.Collections.Generic;


namespace ExBook.Models.Transactions
{
    public class TransactionViewModel
    {
        public Transaction Transaction { get; set; } = null!;
        public BookShelfBook initialBook { get; set; } = null!;
        public ICollection<BookShelfBook> InitiatorBooks { get; set; } = null!;
        public ICollection<BookShelfBook> RecipientBooks { get; set; } = null!;

    }
}
