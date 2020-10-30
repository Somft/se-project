using ExBook.Configuration;
using ExBook.Extensions;
using ExBook.Extensions.DependencyInjection;
using ExBook.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExBook
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.UseConfigurator(this.configuration, new[]
            {
                typeof(AuthenticationConfigurator),
                typeof(DbContextConfigurator),
                typeof(LoggingConfigurator),
                typeof(MailConfigurator),
            });
        }

        /// <summary>
        /// Configures the HTTP request pipeline. 
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsLocal())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStatusCodePages();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<RedirectMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}