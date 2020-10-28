using Microsoft.Extensions.DependencyInjection;

namespace ExBook.Extensions.Configuration
{
    public interface IConfigurator
    {
        void ConfigureServices(IServiceCollection services);
    }
}
