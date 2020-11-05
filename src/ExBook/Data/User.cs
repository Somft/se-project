using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExBook.Data
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("surname")]
        public string Surname { get; set; } = "";

        [Column("password")]
        public string Password { get; set; }

        [Column("role")]
        public string Role { get; set; }


        [Column("is_email_confirmed")]
        public bool IsEmailConfirmed { get; set; }
    }
}
