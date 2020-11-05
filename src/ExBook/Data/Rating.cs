using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBook.Data
{
    [Table("rating")]
    public partial class Rating
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("comment")]
        public string Comment { get; set; }
        [Column("value")]
        public int Value { get; set; }
        [Column("transaction_id")]
        public Guid TransactionId { get; set; }

        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("Rating")]
        public virtual Transaction Transaction { get; set; }
    }
}
