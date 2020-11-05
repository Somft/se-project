using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBook.Data
{
    [Table("transaction")]
    public partial class Transaction
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("status")]
        public string Status { get; set; }

        [InverseProperty("Transaction")]
        public virtual Rating Rating { get; set; }
    }
}
