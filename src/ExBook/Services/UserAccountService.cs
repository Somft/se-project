using ExBook.Data;
using ExBook.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class UserAccountService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserAccountService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

      
        public async Task<User> GetUser(Guid? userId)
        {
            return await this.applicationDbContext.Users
                .FirstOrDefaultAsync(i => i.Id == userId);
        }

        public async Task<User> UpdateData(Guid? userId, User sentUserData)
        {
            User currentUser = await GetUser(sentUserData.Id);
            currentUser.ContactNumber = sentUserData.ContactNumber;
            currentUser.Address = sentUserData.Address;
            currentUser.PostalCode = sentUserData.PostalCode;
            currentUser.City = sentUserData.City;
            currentUser.Country = sentUserData.Country;
            applicationDbContext.SaveChanges();
            return currentUser;
        }

        public void SaveData(User modifiedUser)
        {
            
        }


    }
}
