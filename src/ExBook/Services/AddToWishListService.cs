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

        public async Task<bool> AddBook(AddToWishListViewModel book, Guid user)
        {
            Guid userWishList = this.applicationDbContext.WishList.Where(b => b.UserId == user).Single().Id; // whishlist from current user
            
            if (await this.applicationDbContext.Book.AnyAsync(b => b.Name == book.Name)) //book already exists
            {
                Guid bookId = this.applicationDbContext.Book.Where(b => b.Name == book.Name).Single().Id; // book with the same name
               
                if( await this.applicationDbContext.WishListBook.AnyAsync(b => b.WishListId ==  userWishList && b.BookId == bookId)) // exists in wishlist? throw error
                {
                    return false;
                }
                else // if no, add to wishlist
                {
                    AddAsWish(userWishList, bookId);
                        
                }
            }
            else // book doesnt exists, add to Book and wishlist
            {
               
                this.applicationDbContext.Book.Add(new Book()
                {
                    Id = Guid.NewGuid(),
                    Name = book.Name,
                    Author = book.Author,
                    Created = DateTime.Parse(book.Created)

                });
                
                await this.applicationDbContext.SaveChangesAsync();
                Guid bookId = this.applicationDbContext.Book.Where(b => b.Name == book.Name).Single().Id; // book Id
                AddAsWish(userWishList, bookId);
            }

            

            await this.applicationDbContext.SaveChangesAsync();

            return true;
        }

        public void AddAsWish(Guid userList, Guid bookId)
        {
            this.applicationDbContext.WishListBook.Add(new WishListBook()
            {
                Id = Guid.NewGuid(),
                WishListId = userList,
                BookId = bookId
            });
        }
    }
}
