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
        
        public async Task<bool> AddBook(AddToWishListViewModel book, Guid? user)
        {
            WishList userWishList = this.applicationDbContext.WishLists.FirstOrDefault(b => b.UserId == user); // whishlist from current user
            if ( userWishList == null)
            {
                userWishList = new WishList()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Value
                };
                this.applicationDbContext.Add(userWishList);
            }
            Book bok = this.applicationDbContext.Books.FirstOrDefault(b => b.Name == book.Name);

            if (bok != null) //book already exists
            {
               
                if( await this.applicationDbContext.WishListBooks.AnyAsync(b => b.WishListId ==  userWishList.Id && b.BookId == bok.Id)) // exists in wishlist? throw error
                {
                    return false;
                }
                else // if no, add to wishlist
                {
                    userWishList.WishListBooks.Add(new WishListBook()
                    {
                        Id = Guid.NewGuid(),
                        BookId = bok.Id
                    });

                }
            }
            else // book doesnt exists, add to Book and wishlist
            {
                bok = new Book()
                {
                    Id = Guid.NewGuid(),
                    Name = book.Name,
                    Author = book.Author,
                    Created = DateTime.Parse(book.Created)

                };
                this.applicationDbContext.Books.Add(bok);

                userWishList.WishListBooks.Add(new WishListBook()
                {
                    Id = Guid.NewGuid(),
                    BookId = bok.Id
                }) ;
            }

            await this.applicationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveBook(Guid Id)
        {
            
            WishListBook bok = this.applicationDbContext.WishListBooks.FirstOrDefault(b => b.BookId == Id);
            
            
            this.applicationDbContext.WishListBooks.Remove(bok);
           
            
            await this.applicationDbContext.SaveChangesAsync();

            return true;
        }
    }
}
