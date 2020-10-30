namespace ExBook.Mails.Templates
{
    public class EmailConfirmationContext : EmailContext
    {
        public EmailConfirmationContext()
        {
        }

        public EmailConfirmationContext(string to, string subject) : base(to, subject)
        {
        }
    }
}
