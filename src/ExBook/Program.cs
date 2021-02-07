using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExBook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"ExBook {GetAppVersion()}");
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


                        config.AddInMemoryCollection(new Dictionary<string, string>
                        {
                            { "App:Version", GetAppVersion() }
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
        }

        private static string GetAppVersion()
        {
            AssemblyName assemblyInfo = typeof(Program).Assembly.GetName();
            return "V" + assemblyInfo.Version?.ToString() ?? "0";
        }
    }
}
