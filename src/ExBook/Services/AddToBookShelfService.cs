using ExBook.Data;
using ExBook.Models.AddToBookShelf;
using ExBook.OpenLibrary;
using ExBook.OpenLibrary.Dto;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class AddToBookShelfService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly OpenLibraryClient openLibraryClient;

        public AddToBookShelfService(ApplicationDbContext applicationDbContext, OpenLibraryClient openLibraryClient)
        {
            this.applicationDbContext = applicationDbContext;
            this.openLibraryClient = openLibraryClient;
        }
        
         public async Task<bool> AddBookToBookshelf(AddToBookShelfViewModel book, Guid? user)
        {
            
            BookShelf? userBookShelf = this.applicationDbContext.BookShelves.FirstOrDefault(b => b.UserId == user); // bookshelf from current user
            if ( userBookShelf == null)
            {
                userBookShelf = new BookShelf()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Value
                };
                this.applicationDbContext.Add(userBookShelf);
            }

            book.Name = book.Name.Trim();
            Book bok = this.applicationDbContext.Books.FirstOrDefault(b => b.Name == book.Name);

            if (bok != null) //book already exists
            {

                if (await this.applicationDbContext.BookShelfBooks
                    .AnyAsync(b => b.BookShelfId == userBookShelf.Id && b.BookId == bok.Id && b.IsRemoved == false && b.IsLocked == false )) // exists in bookshelf? throw error
                {
                    return false;
                }
                if (await this.applicationDbContext.BookShelfBooks.AnyAsync(b => b.BookShelfId == userBookShelf.Id && b.BookId == bok.Id && b.IsRemoved == true && b.IsLocked == false ))
                {
                    BookShelfBook ?bookToBringBack = this.applicationDbContext.BookShelfBooks.FirstOrDefault(b =>
                        b.BookShelfId == userBookShelf.Id && b.BookId == bok.Id);
                        bookToBringBack.IsRemoved = false;
                        await this.applicationDbContext.SaveChangesAsync();
                        return true;
                }
                else // if no, add to wishlist
                {
                    userBookShelf.BookShelfBooks.Add(new BookShelfBook()
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
                var bookAPI = await this.SearchBook(book.Name, book.Author);
                if (bookAPI != null)
                {
                    bok2 = this.applicationDbContext.Books.FirstOrDefault(b => b.Name == bookAPI.Title);
                    subjectslist =
                        this.applicationDbContext.Subjects.Where(s => bookAPI.Subjects.Contains(s.Name)).ToList();
                }

                if (bok2 == null) //book doesnt exists in database
                {
                    book.Created = "01.01." + book.Created;
                    DateTime d = DateTime.Parse(book.Created);
                    bok = new Book()
                   {
                       Id = Guid.NewGuid(),
                       Name = bookAPI?.Title ?? book.Name,
                       Author = bookAPI?.Authors.FirstOrDefault().Name ?? book.Author,
                       Created = bookAPI?.FirstPublishDate ?? d,
                       CoverUrl = bookAPI?.Covers.FirstOrDefault().ToString() ?? null,
                       Isbn = bookAPI?.Key ?? null,
                       Subjects = subjectslist ?? null
                       
                       
                   };
                    this.applicationDbContext.Books.Add(bok);

                    userBookShelf.BookShelfBooks.Add(new BookShelfBook()
                    {
                        Id = Guid.NewGuid(),
                        BookId = bok.Id
                    });
                }
                else //book exists in database
                {
                    if (await this.applicationDbContext.BookShelfBooks.AnyAsync(b => b.BookShelfId == userBookShelf.Id && b.BookId == bok2.Id)) // exists in wishlist? throw error
                    {
                        return false;
                    }
                    else // if no, add to wishlist
                    {
                        userBookShelf.BookShelfBooks.Add(new BookShelfBook()
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
         
         public async Task<OpenLibraryBook?> SearchBook(string? title, string? author)
         {
             return await openLibraryClient.SearchBook(title, author);
         }
         
         public async Task<bool> RemoveBook(Guid Id)
         {
            
             BookShelfBook bok = this.applicationDbContext.BookShelfBooks.FirstOrDefault(b => b.BookId == Id);

             bok.IsRemoved = true;

             await this.applicationDbContext.SaveChangesAsync();

             return true;
         }

    }
}