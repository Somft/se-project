using ExBook.Data;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class HomeService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public HomeService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public Task<List<Book>> GetLatestBooksAsync()
        {
            return applicationDbContext.Books.OrderByDescending(b => b.Id).Take(3).ToListAsync();
        }
        public string GetMessage()
        {
            return "Message from service";
        }
    }
}
