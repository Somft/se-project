using ExBook.Data;
using ExBook.Extensions.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExBook.Configuration
{
    public class DbContextConfigurator : IConfigurator
    {
        private readonly IConfiguration configuration;

        public DbContextConfigurator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection databaseConfiguration = this.configuration.GetSection("App:Database");

            string connectionString = databaseConfiguration["ConnectionString"];
            string provider = databaseConfiguration["Provider"];

            switch (provider.ToLower())
            {
                case "postgresql":
                    services.AddDbContext<ApplicationDbContext>(configuration =>
                    {
                        configuration.UseNpgsql(connectionString);
                        configuration.UseLazyLoadingProxies();
                    });
                    break;

                case "sqlite":
                    services.AddDbContext<ApplicationDbContext>(configuration =>
                    {
                        configuration.UseSqlite(connectionString);
                        configuration.UseLazyLoadingProxies();
                    });
                    break;
                default:
                    break;
            }
        }
    }
}
