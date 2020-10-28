using ExBook.Extensions.Configuration;
using ExBook.Mails.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExBook.Configuration
{
    public class MailConfigurator : IConfigurator
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITemplateResolver, EmbeddedTemplateResolver>();
            services.AddTransient<ITemplateEngine, XsltTemplateEngine>();
        }
    }
}
