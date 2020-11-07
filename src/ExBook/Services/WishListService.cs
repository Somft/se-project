using ExBook.Data;

using Microsoft.EntityFrameworkCore;

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

        public async Task<List<WishListBook>> GetUserBook(Guid userId)
        {
            return await applicationDbContext.WishListBook.Include(book => book.Book).Where(book => book.WishList.UserId == userId).ToListAsync();
        }
    }
}
