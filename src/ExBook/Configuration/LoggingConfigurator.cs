using ExBook.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExBook.Configuration
{
    public class LoggingConfigurator : IConfigurator
    {
        private readonly IConfiguration configuration;

        public LoggingConfigurator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.AddConfiguration(this.configuration);

                config.AddConsole(consoleConfig =>
                {
                    consoleConfig.TimestampFormat = "[HH:mm:ss] ";
                });
            });
        }
    }
}
