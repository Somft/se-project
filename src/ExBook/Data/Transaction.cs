using System;
using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class Transaction
    {
        public Transaction()
        {
            this.RecipientBooks = new HashSet<BookShelfBook>();
            this.InitiatorBooks = new HashSet<BookShelfBook>();
        }

        public Guid Id { get; set; }
        public string Status { get; set; }
        public Guid RecipientId { get; set; }
        public Guid InitiatorId { get; set; }

        public virtual User Recipient { get; set; }
        public virtual User Initiator { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<BookShelfBook> RecipientBooks { get; set; }
        public virtual ICollection<BookShelfBook> InitiatorBooks { get; set; }


        public static class Statuses
        {
            public static string Initialized { get; } = "INIT";

            public static string Reviewed { get; } = "REVIEW";

            public static string Rejected { get; } = "REJECTED";

            public static string Accepted { get; } = "ACCEPTED";

            public static string Removed { get; } = "REMOVED";
        }
    }
}
