using ExBook.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class WishListService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public WishListService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
    }
}
