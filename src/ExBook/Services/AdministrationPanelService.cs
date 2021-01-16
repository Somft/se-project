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
            List<User> users = await this.applicationDbContext.Users.ToListAsync();
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
    }
}
