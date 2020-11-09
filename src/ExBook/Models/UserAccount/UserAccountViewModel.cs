using ExBook.Data;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Models.UserAccount
{
    public class UserAccountViewModel
    {
        public User CurrentUser { get; set; }
    }
}
