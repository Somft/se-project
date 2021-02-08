namespace ExBook.Mails.Templates
{
    public class TransactionAcceptedContext : EmailContext
    {
        public TransactionAcceptedContext()
        {
        }

        public TransactionAcceptedContext(string to, string subject) : base(to, subject)
        {
        }
    }
}
