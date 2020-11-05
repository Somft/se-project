﻿namespace ExBook.Mails.Templates
{
    public class EmailContext
    {
        public EmailAddress? To { get; set; } = null;
        public string Subject { get; set; } = "";

        public EmailContext()
        {

        }

        public EmailContext(string subject, string to)
        {
            this.To = new EmailAddress
            {
                Address = to
            };
            this.Subject = subject;
        }
    }
}