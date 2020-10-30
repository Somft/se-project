using MimeKit;

namespace ExBook.Mails
{
    public class EmailAddress
    {
        public string? Name { get; set; }
        public string Address { get; set; } = "";

        public InternetAddress ToInternetAddress()
        {
            return this.Name != null ? new MailboxAddress(this.Name, this.Address) : MailboxAddress.Parse(this.Address);
        }
    }
}
