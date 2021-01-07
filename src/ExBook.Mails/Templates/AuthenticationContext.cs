namespace ExBook.Mails.Templates
{
    public class AuthenticationContext : EmailContext
    {
        public string Token { get; set; } = "";

        public AuthenticationContext(string to, string subject) : base(to, subject)
        {
        }
    }
}
