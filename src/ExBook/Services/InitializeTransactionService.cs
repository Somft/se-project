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

        public async Task<Transaction> GetTransactionById(Guid id)
        {
            Transaction transaction = await this.applicationDbContext.Transactions
                .Include(b => b.Recipient)
                .ThenInclude(b => b.BookShelves)
                .ThenInclude(b => b.BookShelfBooks)
                .ThenInclude(b => b.Book)
                .Include(b => b.Initiator)
                .ThenInclude(b => b.BookShelves)
                .ThenInclude(b => b.BookShelfBooks)
                .ThenInclude(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == id);
            return transaction;

        }



        #region AddTransactionBooks
        public async Task AddBookToInitiatorBooks(Guid bookID, Guid transactionID)
        {

            var transaction = await GetTransactionById(transactionID);
            var book = transaction.InitiatorBooks.FirstOrDefault(rbb => rbb.Id == bookID);
            

            if (book == null)
            {
                var booktoAdd = transaction.Initiator.BookShelves
                    .FirstOrDefault()
                    .BookShelfBooks
                    .FirstOrDefault(bb => bb.Id == bookID);
                transaction.InitiatorBooks.Add(booktoAdd);
            }
            else
            {
                transaction.InitiatorBooks.Remove(book);
               
            }
            await this.applicationDbContext.SaveChangesAsync();
        }

        public async Task AddBookToRecipientBooks(Guid bookID, Guid transactionID)
        {


            var transaction = await GetTransactionById(transactionID);
            var book = transaction.RecipientBooks.FirstOrDefault(rbb => rbb.Id == bookID);

            if(book == null)
            {
                var booktoAdd = transaction.Recipient.BookShelves
                   .FirstOrDefault()
                   .BookShelfBooks
                   .FirstOrDefault(bb => bb.Id == bookID);
                transaction.RecipientBooks.Add(booktoAdd);
            }
            else
            {
                transaction.RecipientBooks.Remove(book);

            }
            await this.applicationDbContext.SaveChangesAsync();
        }
        #endregion AddTransactionBooks


    }
}
