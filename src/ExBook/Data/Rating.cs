using System;
using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class Rating
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public int Value { get; set; }
        public Guid TransactionId { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
