using ExBook.Data;

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

        public List<User> GetUsersByName(string name)
        {
            return this.applicationDbContext.Users.Where(u => u.Name.Contains(name)).ToList();
        }
    }
}
