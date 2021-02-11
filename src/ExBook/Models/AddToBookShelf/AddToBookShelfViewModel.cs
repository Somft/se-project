using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace ExBook.Models.AddToBookShelf
{
    public class AddToBookShelfViewModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(60)]
        public string Name { get; set; } = "";

        [Required]
        [MinLength(1)]
        [MaxLength(60)]
        public string Author { get; set; } = "";

        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        public string Created { get; set; } = "";

        public string? Message { get; set; } = null;

        public bool Success { get; set; } = false;

        public IFormFile? Photo { get; set; } = null;
    }
}