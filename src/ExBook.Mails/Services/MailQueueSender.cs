using ExBook.Mails.Templates;
using System;
using System.Threading.Tasks;

namespace ExBook.Mails.Services
{
    public class MailQueueSender : IMailSender
    {
        private readonly ITemplateEngine templateEngine;
        private readonly MailQueueDbContext mailQueueDbContext;

        public MailQueueSender(ITemplateEngine templateEngine, MailQueueDbContext mailQueueDbContext)
        {
            this.templateEngine = templateEngine;
            this.mailQueueDbContext = mailQueueDbContext;
        }

        public async Task SendEmail<T>(string template, T context) where T : EmailContext
        {
            string content = await this.templateEngine.Render<T>(template, context);

            var newMail = new Mail
            {
                Id = Guid.NewGuid(),
                Content = content,
                Error = null,
                To = context.To?.Address ?? "",
                Subject = context.Subject,
            };

            await mailQueueDbContext.Mails.AddAsync(newMail);
            await mailQueueDbContext.SaveChangesAsync();
        }
    }
}
