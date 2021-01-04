using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

using System.Linq;

namespace ExBook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((config) =>
                    {
                        config.Sources
                         .Select(source => source as JsonConfigurationSource)
                         .Where(source => source != null)
                         .ToList()
                         .ForEach(source => config.AddJsonFile(source!.Path.Replace(".json", "") + ".secrets.json", true));

                    });
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
