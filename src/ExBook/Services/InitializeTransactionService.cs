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
    public class InitializeTransactionService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public InitializeTransactionService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<List<BookShelfBook>>GetUserBookShelfBooks(Guid id)
        {
            List<BookShelfBook> userBookShelfBooks = await this.applicationDbContext.BookShelfBooks
                 .Include(bsb => bsb.BookShelf)
                 .ThenInclude(bs => bs.User)
                 .Include(bsb => bsb.Book)
                 .Where(bsb => bsb.BookShelf.UserId == id)
                 .ToListAsync();
            return userBookShelfBooks;
        }

        public async Task<User> GetUserById(Guid id)
        {
            User user = await this.applicationDbContext.Users
                .Include(u => u.BookShelves)
                .ThenInclude(u => u.BookShelfBooks)
                .ThenInclude(u => u.Book)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task <BookShelfBook> GetBookShelfBookById(Guid id)
        {
            BookShelfBook bookshelf  = await this.applicationDbContext.BookShelfBooks
               .Include(bsb => bsb.BookShelf)
               .ThenInclude(bs => bs.User)
               .Include(bsb => bsb.Book)
               .FirstOrDefaultAsync(bsb => bsb.Id == id);
            return bookshelf;
        }

        #region TransactionBooks
        public void AddBookToInititatorBooks()
        {

        }

        public async Task AddBookToRecipientBooks(Guid bookID, Guid transactionID)
        {


             var transaction = await this.applicationDbContext.Transactions
                .FirstOrDefaultAsync(trans => trans.Id == transactionID);
            var book = transaction.RecipientBooks.FirstOrDefault(rbb => rbb.BookId == bookID);

            if(book == null)
            {
                transaction.RecipientBooks.Add(book);
            }
            else
            {
                transaction.RecipientBooks.Remove(book);
            }
        }
        #endregion TransactionBooks


    }
}
