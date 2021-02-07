using ExBook.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ExBook.Models.Transactions
{
    public class TransactionRatingViewModel
    {
        public Transaction? Transaction { get; set; } = null;

        [MaxLength(200)]
        public string? Comments { get; set; } = null;

        [Required]
        [Range(1, 10)]
        public int? Rating { get; set; } = null;
    }
}
