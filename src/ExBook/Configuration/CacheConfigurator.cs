using ExBook.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExBook.Configuration
{
    public class CacheConfigurator : IConfigurator
    {
        private readonly IConfiguration configuration;

        public CacheConfigurator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
        }
    }
}
