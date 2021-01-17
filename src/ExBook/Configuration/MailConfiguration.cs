using ExBook.Extensions.Configuration;
using ExBook.Mails;
using ExBook.Mails.Services;

using Microsoft.EntityFrameworkCore;
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

            if (this.configuration.GetValue("App:Mail:QueueEnabled", true))
            {


                services.AddTransient<IMailSender, MailQueueSender>();

                IConfigurationSection databaseConfiguration = this.configuration.GetSection("App:Database");

                string connectionString = databaseConfiguration["ConnectionString"];
                string provider = databaseConfiguration["Provider"];

                services.AddDbContext<MailQueueDbContext>(configuration =>
                {
                    switch (provider.ToLower())
                    {
                        case "postgresql":
                            configuration.UseNpgsql(connectionString);
                            break;

                        case "sqlite":
                            configuration.UseSqlite(connectionString);
                            break;
                        default:
                            break;
                    }
                });

                if (this.configuration.GetValue("App:Mail:BackgroundSenderEnabled", true))
                {
                    services.AddHostedService<MailBackgroundSender>();
                    services.AddScoped<MailSender>();
                }
            }
            else
            {
                services.AddTransient<IMailSender, MailSender>();
            }

            services.AddTransient<ITemplateResolver, EmbeddedTemplateResolver>();
            services.AddTransient<ITemplateEngine, XsltTemplateEngine>();
        }
    }
}
