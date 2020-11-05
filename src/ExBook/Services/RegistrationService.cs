using ExBook.Data;
using ExBook.Mails.Services;
using ExBook.Mails.Templates;
using ExBook.Models.Authentication;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class RegistrationService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMailSender mailSender;

        public RegistrationService(ApplicationDbContext applicationDbContext, IMailSender mailSender)
        {
            this.applicationDbContext = applicationDbContext;
            this.mailSender = mailSender;
        }

        public async Task<bool> RegisterUser(RegisterViewModel userData)
        {
            if (await this.applicationDbContext.Users.AnyAsync(u => u.Login == userData.Login))
            {
                return false;
            }

            this.applicationDbContext.Users.Add(new User()
            {
                Id = Guid.NewGuid(),
                Login = userData.Login,
                Email = userData.Email,
                Name = userData.Name,
                Surname = userData.Surname,
                Password = userData.Password,
                Role = "user",
                IsEmailConfirmed = false,
            });
            await this.applicationDbContext.SaveChangesAsync();

            await this.mailSender.SendEmail("EmailConfirmation", new EmailConfirmationContext("Account confirmation", userData.Email) { });

            return true;
        }
    }
}
