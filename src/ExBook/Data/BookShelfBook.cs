using System;
using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class BookShelfBook
    {
        public BookShelfBook()
        {
            this.InitiatorTransactions = new HashSet<Transaction>();
            this.RecipientTransactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public Guid? Photo { get; set; }
        public bool IsLocked { get; set; }  
        public bool IsRemoved { get; set; }
        public Guid BookId { get; set; }
        public Guid BookShelfId { get; set; }

        public virtual Book Book { get; set; }
        public virtual BookShelf BookShelf { get; set; }
        public virtual ICollection<Transaction> InitiatorTransactions { get; set; }
        public virtual ICollection<Transaction> RecipientTransactions { get; set; }
    }
}
