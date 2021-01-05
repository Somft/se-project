using ExBook.Mails.Templates;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ExBook.Mails.Services
{
    public class MailBackgroundSender : BackgroundService
    {
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        public MailBackgroundSender(ILogger<MailBackgroundSender> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogDebug("Background mail sender enabled");

            while (!stoppingToken.IsCancellationRequested)
            {
                using IServiceScope scope = this.serviceProvider.CreateScope();
                await this.SendEmails(scope, stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            this.logger.LogInformation("Shutting down mail sender");
        }

        private async Task SendEmails(IServiceScope scope, CancellationToken stoppingToken)
        {
            MailQueueDbContext dbContext = scope.ServiceProvider.GetRequiredService<MailQueueDbContext>();
            MailSender mailSender = scope.ServiceProvider.GetRequiredService<MailSender>();

            Expression<Func<Mail, bool>> query = mail => mail.Error == null && !mail.Success;

            while (await dbContext.Mails.AnyAsync(query))
            {
                using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
                Mail mailToSend = await dbContext.Mails.FirstAsync(query);

                try
                {
                    await mailSender.SendEmail(mailToSend.Content, new EmailContext(mailToSend));
                    mailToSend.Success = true;
                    mailToSend.Sent = DateTime.UtcNow;
                }
                catch (Exception e)
                {
                    mailToSend.Error = e.Message;
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }
    }
}
