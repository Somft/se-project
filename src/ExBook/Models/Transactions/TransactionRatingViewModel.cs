using ExBook.Data;
using System.ComponentModel.DataAnnotations;

namespace ExBook.Models.Transactions
{
    public class TransactionRatingViewModel
    {
        public Transaction Transaction { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Comments { get; set; } = "";

        [Required]
        [Range(1, 10)]
        public int? Rating { get; set; } = null;
    }
}
