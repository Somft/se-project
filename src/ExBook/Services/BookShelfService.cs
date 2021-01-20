using ExBook.Data;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ExBook.Services
{
    public class BookShelfService
    {
        private readonly ApplicationDbContext applicationDbContext;
        
        public BookShelfService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<List<BookShelfBook>> GetUserBook(Guid userId)
        {
            return await applicationDbContext.BookShelfBooks.Include(book => book.Book)
                .Where(book => book.BookShelf.UserId == userId && book.IsLocked == false && book.IsRemoved == false ).ToListAsync();
        }
    }
}