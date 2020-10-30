using ExBook.Mails.Templates;

using System.Threading.Tasks;

namespace ExBook.Mails.Services
{
    public interface IMailSender
    {
        public Task SendEmail<T>(string template, T context) where T : EmailContext;
    }
}