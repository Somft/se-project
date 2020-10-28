using System.Threading.Tasks;

namespace ExBook.Mails.Services
{
    public interface ITemplateResolver
    {
        Task<string> GetTemplate(string name);
    }
}