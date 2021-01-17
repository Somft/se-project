using ExBook.Mails.Services;

namespace ExBook.Mails.Templates
{
    public class EmailContext
    {
        public EmailAddress? To { get; set; } = null;
        public string Subject { get; set; } = "";

        public EmailContext()
        {

        }

        public EmailContext(string to, string subject)
        {
            this.To = new EmailAddress
            {
                Address = to
            };
            this.Subject = subject;
        }

        public EmailContext(Mail mailToSend) : this(mailToSend.Subject, mailToSend.To)
        {

        }
    }
}
