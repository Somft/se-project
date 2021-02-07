using ExBook.Data;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class AdministrationPanelService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AdministrationPanelService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<List<User>> GetUsers()
        {
            List<User> users = await this.applicationDbContext.Users
                .Include(u=>u.InitiatedTransactions)
                .Include(u=>u.ReceivedTransactions)
                .Include(u=>u.BookShelves)
                .ToListAsync();
            return users;
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            List<Transaction> transactions = await this.applicationDbContext.Transactions
                .Include(t => t.Initiator)
                .Include(t => t.Recipient)
                .ToListAsync();
            return transactions;
        }

        public async Task<List<WishListBook>> GetUserWishlist(Guid id)
        {
            List<WishListBook> books = await this.applicationDbContext.WishListBooks
                .Include(t => t.Book)
                .ThenInclude(t => t.Subjects)
                .Where(t=> t.WishListId==id)
                .ToListAsync();
            return books;
        }

        public async Task<List<BookShelfBook>> GetUserBookshelf(Guid id)
        {
            List<BookShelfBook> books = await this.applicationDbContext.BookShelfBooks
                .Include(t => t.Book)
                .ThenInclude(t => t.Subjects)
                .Where(t => t.BookShelfId == id)
                .ToListAsync();
            return books;
        }

        public async Task<List<Rating>> GetRatings()
        {
            List<Rating> ratings = await this.applicationDbContext.Ratings
                .Include(r => r.Transaction)
    .ThenInclude(t => t.Initiator)
    .Include(r => r.Transaction)
    .ThenInclude(t => t.Recipient)
    .ToListAsync();
            return ratings;
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await this.applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<BookShelfBook> GetBookshelfBookById(Guid id)
        {
            return await this.applicationDbContext.BookShelfBooks.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<WishListBook> GetWishlistBookById(Guid id)
        {
            return await this.applicationDbContext.WishListBooks.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task DeleteUserById(Guid id)
        {
            var user = await this.applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                this.applicationDbContext.Remove(user);
                await this.applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteBookshelfBookById(Guid id)
        {
            var book = await this.applicationDbContext.BookShelfBooks
                .Include(b => b.InitiatorTransactions)
                .Include(b => b.RecipientTransactions)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (book != null)
            {
                this.applicationDbContext.Remove(book);
                await this.applicationDbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteWishlistBookById(Guid id)
        {
            var book = await this.applicationDbContext.WishListBooks.FirstOrDefaultAsync(u => u.Id == id);
            if (book != null)
            {
                this.applicationDbContext.Remove(book);
                await this.applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task<Transaction> GetTransactionById(Guid id)
        {
            return await this.applicationDbContext.Transactions.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task DeleteTransactionById(Guid id)
        {
            var transaction = await this.applicationDbContext.Transactions
                .Include(t=>t.InitiatorBooks)
                .Include(t=>t.RecipientBooks)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (transaction != null)
            {
                this.applicationDbContext.Remove(transaction);
                await this.applicationDbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateUser(User sentUserData)
        {
            User currentUser = await GetUserById(sentUserData.Id);
            currentUser.Login = sentUserData.Login;
            currentUser.Email = sentUserData.Email;
            currentUser.Password = sentUserData.Password;
            currentUser.Role = sentUserData.Role;
            currentUser.Name = sentUserData.Name;
            currentUser.Surname = sentUserData.Surname;
            currentUser.ContactNumber = sentUserData.ContactNumber;
            currentUser.Address = sentUserData.Address;
            currentUser.PostalCode = sentUserData.PostalCode;
            currentUser.City = sentUserData.City;
            currentUser.Country = sentUserData.Country;
            await this.applicationDbContext.SaveChangesAsync();
        }
    }
}
