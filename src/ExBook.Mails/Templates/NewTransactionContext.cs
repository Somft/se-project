namespace ExBook.Mails.Templates
{
    public class NewTransactionContext : EmailContext
    {
        public NewTransactionContext()
        {
        }

        public NewTransactionContext(string to, string subject) : base(to, subject)
        {
        }
    }
}
