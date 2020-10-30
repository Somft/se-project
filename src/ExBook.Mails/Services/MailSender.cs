using ExBook.Mails.Templates;

using Microsoft.Extensions.Options;

using MimeKit;

using System.Threading.Tasks;

namespace ExBook.Mails.Services
{
    public class MailSender : IMailSender
    {
        private readonly ITemplateEngine templateEngine;
        private readonly MailSettings configuration;

        public MailSender(ITemplateEngine templateEngine, IOptions<MailSettings> configuration)
        {
            this.configuration = configuration.Value;
            this.templateEngine = templateEngine;
        }

        public async Task SendEmail<T>(string template, T context) where T : EmailContext
        {
            string content = await this.templateEngine.Render<T>(template, context);
            await this.SendEmail(context, content);
        }

        private async Task SendEmail<T>(T context, string content) where T : EmailContext
        {
            var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            smtpClient.Connect(this.configuration.Host, this.configuration.Port, this.configuration.UseSsl);
            if (this.configuration.User != null || this.configuration.Password != null)
            {
                smtpClient.Authenticate(this.configuration.User, this.configuration.Password);
            }
            smtpClient.Send(this.CreateMimeMessage(context, content));
            smtpClient.Disconnect(true);
        }

        private MimeMessage CreateMimeMessage(EmailContext context, string content)
        {


            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(this.configuration.From?.ToInternetAddress());
            mimeMessage.To.Add(context.To?.ToInternetAddress());
            mimeMessage.Subject = context.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = content
            };
            return mimeMessage;
        }
    }
}
