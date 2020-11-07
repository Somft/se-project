using ExBook.Data;
using ExBook.Models.AddToWhishList;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class AddToWishListService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AddToWishListService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<bool> AddBook(AddToWishListViewModel book)
        {


            this.applicationDbContext.Book.Add(new Book()
            {
                Id = Guid.NewGuid(),
                Name = book.Name,
                Author = book.Author,
                Created = DateTime.Parse(book.Created)

            }) ;

            await this.applicationDbContext.SaveChangesAsync();

            return true;
        }
        /* public async Task<List<WishListBook>> GetUserBook(Guid userId)
         {
             return await applicationDbContext.WishListBook.Include(book => book.Book).Where(book => book.WishList.UserId == userId).ToListAsync();
         }*/
    }
}
