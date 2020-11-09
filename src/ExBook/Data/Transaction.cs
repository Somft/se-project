using System;

#nullable disable

namespace ExBook.Data
{
    public partial class Transaction
    {
        public Guid Id { get; set; }
        public string Status { get; set; }

        public virtual Rating Rating { get; set; }
    }
}
