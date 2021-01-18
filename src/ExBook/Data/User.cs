using System;
using System.Collections.Generic;

#nullable disable

namespace ExBook.Data
{
    public partial class User
    {
        public User()
        {
            this.BookShelves = new HashSet<BookShelf>();
            this.WishLists = new HashSet<WishList>();
            this.InitiatedTransactions = new HashSet<Transaction>();
            this.ReceivedTransactions = new HashSet<Transaction>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Surname { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsEmailAuthenticationEnabled { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }


        public virtual ICollection<BookShelf> BookShelves { get; set; }
        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ICollection<Transaction> InitiatedTransactions { get; set; }
        public virtual ICollection<Transaction> ReceivedTransactions { get; set; }
    }
}
