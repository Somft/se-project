using ExBook.Data;
using ExBook.Mails.Services;
using ExBook.Mails.Templates;
using ExBook.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Threading.Tasks;

namespace ExBook.Services
{
    public class RegistrationService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMailSender mailSender;
        private readonly IConfiguration configuration;
        private readonly IPasswordHasher<User> passwordHasher;

        public RegistrationService(ApplicationDbContext applicationDbContext, IMailSender mailSender, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            this.applicationDbContext = applicationDbContext;
            this.mailSender = mailSender;
            this.configuration = configuration;
            this.passwordHasher = passwordHasher;
        }

        public async Task<bool> RegisterUser(RegisterViewModel userData)
        {
            if (await this.applicationDbContext.Users.AnyAsync(u => u.Login == userData.Login))
            {
                return false;
            }

            User user = this.applicationDbContext.Users.Add(new User()
            {
                Id = Guid.NewGuid(),
                Login = userData.Login,
                Email = userData.Email,
                Name = userData.Name,
                Surname = userData.Surname,
                Role = "user",
                IsEmailConfirmed = false,
            }).Entity;
            user.Password = passwordHasher.HashPassword(user, userData.Password);

            await this.applicationDbContext.SaveChangesAsync();

            await this.mailSender.SendEmail(
                "EmailConfirmation",
                new EmailConfirmationContext(userData.Email, "Account confirmation")
                {
                    Url = this.configuration["App:Url"] + "confirm?key=" + user.Id,
                });

            return true;
        }


        public async Task ConfirmRegistration(Guid userId)
        {
            User user = await this.applicationDbContext.Users.SingleOrDefaultAsync(u => u.Id == userId);
            user.IsEmailConfirmed = true;
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
