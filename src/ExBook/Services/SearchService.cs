using ExBook.Data;

using LinqKit;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class SearchService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public SearchService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        #region Books
        public async Task<List<Book>> GetBooksFiltered(string filterTitle, string filterAuthor, bool filterAvailable)
        {
            Expression<Func<Book, bool>> filter = PredicateBuilder.True<Book>();

            if (!string.IsNullOrEmpty(filterTitle))
                filter = filter.And(b => b.Name.Contains(filterTitle));

            if (!string.IsNullOrEmpty(filterAuthor))
                filter = filter.And(b => b.Author.Contains(filterAuthor));

            if (filterAvailable)
                filter = filter.And(b => b.BookShelfBooks.Count > 0);

            List<Book> books = await this.applicationDbContext.Books
                .Include(b => b.BookShelfBooks)
                .Where(filter)
                .OrderByDescending(b => b.BookShelfBooks.Count)
                .ToListAsync();
            return books;
        }

        public async Task<List<Book>> GetAllAvailableBooks()
        {
            List<Book> books = await this.applicationDbContext.Books
                .Include(b => b.BookShelfBooks)
                .Include(b => b.WishListBooks)
                .ThenInclude(wlb => wlb.WishList)
                .OrderByDescending(b => b.BookShelfBooks.Count)
                .ToListAsync();
            return books;
        }
        #endregion Books

        #region BookshelfBooks
        public async Task<List<BookShelfBook>> GetAllBookShelfBooks()
        {
            List<BookShelfBook> bookShelfBooks = await this.applicationDbContext.BookShelfBooks
                .Include(bsb => bsb.BookShelf)
                .ThenInclude(bs=>bs.User)
                .Include(bsb=> bsb.Book)              
                .ToListAsync();
            return bookShelfBooks;
        }
        public async Task<List<BookShelfBook>> GetBookShelfBooksByTitle(string title)
        {
            List<BookShelfBook> bookShelfBooks = await this.applicationDbContext.BookShelfBooks
                .Include(bsb => bsb.BookShelf)
                .ThenInclude(bs => bs.User)
                .Include(bsb => bsb.Book)
                .Where(bsb=> bsb.Book.Name.Contains(title))
                .ToListAsync();
            return bookShelfBooks;
        }

        public async Task<List<BookShelfBook>> GetBookShelfBooksById(Guid Id)
        {
            List<BookShelfBook> bookShelfBooks = await this.applicationDbContext.BookShelfBooks
                .Include(bsb => bsb.BookShelf)
                .ThenInclude(bs => bs.User)
                .Include(bsb => bsb.Book)
                .Where(bsb => bsb.Book.Id== Id)          
                .ToListAsync();
            return bookShelfBooks;
        }

        public async Task<List<BookShelfBook>> GetBookShelfBooksFiltered(string filterTitle, string filterLogin)
        {
            Expression<Func<BookShelfBook, bool>> filter = PredicateBuilder.True<BookShelfBook>();

            if (!string.IsNullOrEmpty(filterTitle))
                filter = filter.And(bsb => bsb.Book.Name.Contains(filterTitle));

            if (!string.IsNullOrEmpty(filterLogin))
                filter = filter.And(bsb => bsb.BookShelf.User.Login.Contains(filterLogin));


            List<BookShelfBook> bookShelfBooks = await this.applicationDbContext.BookShelfBooks
                .Include(bsb => bsb.BookShelf)
                .ThenInclude(bs => bs.User)
                .Include(bsb => bsb.Book)
                .Where(filter)
                .ToListAsync();
            return bookShelfBooks;
        }

        #endregion  BookshelfBooks

        #region WishList
        public async Task AddToWishList(Guid bookId, Guid guid)
        {
            var wishlist = await applicationDbContext.WishLists
                .Include(wsl => wsl.WishListBooks)
                .FirstOrDefaultAsync(wsl => wsl.UserId == guid);

            if (wishlist == null)
            {
                wishlist = new WishList()
                {
                    Id = Guid.NewGuid(),
                    UserId = guid
                };
             await  this.applicationDbContext.AddAsync(wishlist);
            }
            var book = wishlist.WishListBooks.FirstOrDefault(wlb => wlb.BookId == bookId);
            // if exists - remove from wishlist
            if (book!=null)
            {
                wishlist.WishListBooks.Remove(book);
                this.applicationDbContext.Remove(book);
            }
            else
            {
                wishlist.WishListBooks.Add(new WishListBook()
                {
                    Id = Guid.NewGuid(),
                    BookId = bookId
                });
            }
            await this.applicationDbContext.SaveChangesAsync();       
        }
        #endregion WishList
    }
}
