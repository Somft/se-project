﻿
using System;

namespace ExBook.Mails.Services
{
    public class Mail
    {
        public Guid Id { get; set; }

        public string To { get; set; } = "";

        public string Subject { get; set; } = "";

        public string Content { get; set; } = "";


        public string? Error { get; set; } = null;

        public bool Success { get; set; } = false;

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime? Sent { get; set; } = null;

        public string? Owner { get; set; } = null;
    }
}
