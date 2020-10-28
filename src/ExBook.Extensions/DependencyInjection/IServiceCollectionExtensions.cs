using ExBook.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace ExBook.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection UseConfigurator(this IServiceCollection services, IConfiguration configuration, params Type[] types)
        {

            foreach (Type type in types)
            {
                services.UseConfigurator(configuration, type);
            }

            return services;
        }

        public static IServiceCollection UseConfigurator(this IServiceCollection services, IConfiguration configuration, Type type)
        {
            if (type.GetConstructor(new[] { typeof(IConfiguration) }) != null)
            {
                IConfigurator v = Activator.CreateInstance(type, configuration) as IConfigurator ?? throw new ArgumentException();
                v.ConfigureServices(services);
            }
            else
            {
                IConfigurator v = Activator.CreateInstance(type) as IConfigurator ?? throw new ArgumentException();
                v.ConfigureServices(services);
            }



            return services;
        }
    }
}
