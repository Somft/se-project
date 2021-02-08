namespace ExBook.Mails.Templates
{
    public class TransactionRejectedContext : EmailContext
    {
        public TransactionRejectedContext()
        {
        }

        public TransactionRejectedContext(string to, string subject) : base(to, subject)
        {
        }
    }
}
