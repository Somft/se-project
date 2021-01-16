using ExBook.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Models.AdministrationPanel
{
    public class AdministrationPanelViewModel
    {
        public List<User> Users { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<Rating> Ratings { get; set; }
    }
}
