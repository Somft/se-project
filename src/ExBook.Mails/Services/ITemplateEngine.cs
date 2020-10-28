using ExBook.Mails.Templates;
using System.Threading.Tasks;

namespace ExBook.Mails.Services
{
    public interface ITemplateEngine
    {
        Task<string> Render<T>(string template, T context) where T : BaseContext;
    }
}