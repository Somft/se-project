using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBook.Data
{
    [Table("user")]
    public partial class User
    {
        public User()
        {
            BookShelf = new HashSet<BookShelf>();
            WishList = new HashSet<WishList>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("email")]
        public string Email { get; set; }
        [Required]
        [Column("login")]
        public string Login { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Required]
        [Column("password")]
        public string Password { get; set; }
        [Required]
        [Column("role")]
        public string Role { get; set; }
        [Required]
        [Column("surname")]
        public string Surname { get; set; }
        [Column("is_email_confirmed")]
        public bool IsEmailConfirmed { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<BookShelf> BookShelf { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<WishList> WishList { get; set; }
    }
}
