using System.Collections.Generic;
using ExBook.Data;

namespace ExBook.Models.Transactions
{
    public class UserTransactionsViewModel
    {
        public List<Transaction> ToReview { get; set; } = new List<Transaction>();

        public List<Transaction> Waiting { get; set; } = new List<Transaction>();

        public List<Transaction> Rejected { get; set; } = new List<Transaction>();
    }
}
