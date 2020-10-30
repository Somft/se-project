namespace ExBook.Mails
{
    public class MailSettings
    {
        public bool UseSsl { get; set; } = false;

        public string Host { get; set; } = "localhost";

        public int Port { get; set; } = 25;

        public EmailAddress? From { get; set; }

        public string? User { get; set; }

        public string? Password { get; set; }
    }
}
