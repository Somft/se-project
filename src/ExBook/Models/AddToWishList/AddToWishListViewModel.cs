using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ExBook.Models.AddToWhishList
{
    public class AddToWishListViewModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string Name { get; set; } = "";

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string Author { get; set; } = "";

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string Created { get; set; } = "";

    }

}