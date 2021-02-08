namespace ExBook.Mails.Templates
{
    public class EmailConfirmationContext : EmailContext
    {
        public string Url { get; set; } = "";


        public EmailConfirmationContext()
        {
        }

        public EmailConfirmationContext(string to, string subject) : base(to, subject)
        {
        }
    }
}
