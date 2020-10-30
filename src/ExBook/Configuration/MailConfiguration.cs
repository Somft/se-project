using ExBook.Extensions.Configuration;
using ExBook.Mails;
using ExBook.Mails.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExBook.Configuration
{
    public class MailConfigurator : IConfigurator
    {
        private readonly IConfiguration configuration;

        public MailConfigurator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection identitySettingsSection = this.configuration.GetSection("App:Mail");
            services.Configure<MailSettings>(identitySettingsSection);

            services.AddTransient<IMailSender, MailSender>();
            services.AddTransient<ITemplateResolver, EmbeddedTemplateResolver>();
            services.AddTransient<ITemplateEngine, XsltTemplateEngine>();
        }
    }
}
