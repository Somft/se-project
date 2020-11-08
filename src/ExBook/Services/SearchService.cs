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

        public List<User> GetAllUsers()
        {
            return this.applicationDbContext.Users.ToList();
        }

        public async Task<List<User>> GetUsersByName(string name)
        {
            List<User> users = await this.applicationDbContext.Users.Where(u => u.Name.Contains(name)).ToListAsync();
            return users;
        }


        public async Task<List<BookShelfBook>> GetAllBookShelfBooks()
        {
            List<BookShelfBook> bookShelfBooks = await this.applicationDbContext.BookShelfBook
                .Include(bsb => bsb.BookShelf)
                .ThenInclude(bs=>bs.User)
                .Include(bsb=> bsb.Book)              
                .ToListAsync();
            return bookShelfBooks;
        }
        public async Task<List<BookShelfBook>> GetBookShelfBooksByTitle(string title)
        {
            List<BookShelfBook> bookShelfBooks = await this.applicationDbContext.BookShelfBook
                .Include(bsb => bsb.BookShelf)
                .ThenInclude(bs => bs.User)
                .Include(bsb => bsb.Book)
                .Where(bsb=> bsb.Book.Name.Contains(title))
                .ToListAsync();
            return bookShelfBooks;
        }

        public async Task<List<BookShelfBook>> GetBookShelfBooksById(Guid Id)
        {
            List<BookShelfBook> bookShelfBooks = await this.applicationDbContext.BookShelfBook
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


            List<BookShelfBook> bookShelfBooks = await this.applicationDbContext.BookShelfBook
                .Include(bsb => bsb.BookShelf)
                .ThenInclude(bs => bs.User)
                .Include(bsb => bsb.Book)
                .Where(filter)
                .ToListAsync();
            return bookShelfBooks;
        }

        public async Task<List<Book>> GetBooksFiltered(string filterTitle, string filterAuthor, bool filterAvailable)
        {
            Expression<Func<Book, bool>> filter = PredicateBuilder.True<Book>();

            if (!string.IsNullOrEmpty(filterTitle))
                filter = filter.And(b => b.Name.Contains(filterTitle));

            if (!string.IsNullOrEmpty(filterAuthor))
                filter = filter.And(b => b.Author.Contains(filterAuthor));

            if(filterAvailable)
            filter = filter.And(b => b.BookShelfBook.Count>0);

            List<Book> books = await this.applicationDbContext.Book
                .Include(b=>b.BookShelfBook)
                .Where(filter)
                .OrderByDescending(b=>b.BookShelfBook.Count)
                .ToListAsync();
            return books;
        }

        public async Task<List<Book>> GetAllAvailableBooks()
        {
            List<Book> books = await this.applicationDbContext.Book
                .Include(b => b.BookShelfBook)
                .Include(b => b.WishListBook)
                .ThenInclude(wlb=> wlb.WishList)
                .OrderByDescending(b => b.BookShelfBook.Count)
                .ToListAsync();
            return books;
        }

        internal void AddToWishList(Guid bookId, Guid guid)
        {
            var wishlist = applicationDbContext.WishList
                .Include(wsl => wsl.WishListBook)
                .FirstOrDefault(wsl => wsl.UserId == guid);

            if (wishlist == null)
            {
                WishList newWishList = new WishList()
                {
                    Id = Guid.NewGuid(),
                    UserId = guid
                };
                newWishList.WishListBook.Add(new WishListBook()
                {
                    BookId = bookId
                });
                 this.applicationDbContext.Add(newWishList);
                this.applicationDbContext.SaveChanges();
            }
            else
            {
                wishlist.WishListBook.Add(new WishListBook()
                {
                    Id = Guid.NewGuid(),
                    BookId = bookId
                });
                this.applicationDbContext.SaveChanges();
            }

        }
    }
}
