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
        public async Task<List<Book>> GetLatestBooksAsync()
        {
            var books = await this.applicationDbContext.Books.ToListAsync();
            return books.Skip(Math.Max(0, books.Count - 3)).ToList();
        }
        public string GetMessage()
        {
            return "Message from service";
        }
    }
}
