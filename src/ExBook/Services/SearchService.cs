using ExBook.Data;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<BookShelfBook>> GetBookShelfBooksFiltered(string filterTitle, string filterLogin)
        {
            List<BookShelfBook> bookShelfBooks = await this.applicationDbContext.BookShelfBook
                .Include(bsb => bsb.BookShelf)
                .ThenInclude(bs => bs.User)
                .Include(bsb => bsb.Book)
                .Where(bsb => bsb.Book.Name.Contains(filterTitle) && bsb.BookShelf.User.Login.Contains(filterLogin))
                .ToListAsync();
            return bookShelfBooks;
        }
    }
}
