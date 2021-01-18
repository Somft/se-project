using ExBook.Data;
using ExBook.Models.AddToWhishList;
using ExBook.OpenLibrary;
using ExBook.OpenLibrary.Dto;

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
        private readonly OpenLibraryClient openLibraryClient;

        public AddToWishListService(ApplicationDbContext applicationDbContext, OpenLibraryClient openLibraryClient)
        {
            this.applicationDbContext = applicationDbContext;
            this.openLibraryClient = openLibraryClient;
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

                if (await this.applicationDbContext.WishListBooks.AnyAsync(b => b.WishListId == userWishList.Id && b.BookId == bok.Id)) // exists in wishlist? throw error
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
                Book bok2 = null;
                List<Subject> subjectslist = null;
                var bookAPI = await SearchBook(book.Name, book.Author);
                if (bookAPI != null)
                {
                    bok2 = this.applicationDbContext.Books.FirstOrDefault(b => b.Name == bookAPI.Title);
                    subjectslist =
                        this.applicationDbContext.Subjects.Where(s => bookAPI.Subjects.Contains(s.Name)).ToList();
                }

                if (bok2 == null) //book doesnt exists in database
                {
                    bok = new Book()
                   {
                       Id = Guid.NewGuid(),
                       Name = bookAPI?.Title ?? book.Name,
                       Author = bookAPI?.Authors.FirstOrDefault().Name ?? book.Author,
                       Created = bookAPI?.FirstPublishDate ?? DateTime.Parse(book.Created),
                       CoverUrl = bookAPI?.Covers.FirstOrDefault().ToString() ?? null,
                       Isbn = bookAPI?.Key ?? null,
                       Subjects = subjectslist ?? null
                       
                       
                   };
                    this.applicationDbContext.Books.Add(bok);

                    userWishList.WishListBooks.Add(new WishListBook()
                    {
                        Id = Guid.NewGuid(),
                        BookId = bok.Id
                    });
                }
                else //book exists in database
                {
                    if (await this.applicationDbContext.WishListBooks.AnyAsync(b => b.WishListId == userWishList.Id && b.BookId == bok2.Id)) // exists in wishlist? throw error
                    {
                        return false;
                    }
                    else // if no, add to wishlist
                    {
                        userWishList.WishListBooks.Add(new WishListBook()
                        {
                            Id = Guid.NewGuid(),
                            BookId = bok2.Id
                        });

                    }


                }

                
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

        public async Task<OpenLibraryBook?> SearchBook(string? title, string? author)
        {
            return await openLibraryClient.SearchBook(title, author);
        }
    }
}
