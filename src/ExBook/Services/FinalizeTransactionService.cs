using ExBook.Data;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class FinalizeTransactionService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public FinalizeTransactionService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Transaction> GetTransactionById(Guid id)
        {
                Transaction transaction = await this.applicationDbContext.Transactions
                    .FirstOrDefaultAsync(transaction => transaction.Id == id);
                return transaction; 
        }
    }

    //todo
    //return button
}
