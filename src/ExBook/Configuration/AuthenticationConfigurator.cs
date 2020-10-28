using ExBook.Extensions.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using System.Threading.Tasks;

namespace ExBook.Configuration
{
    public class AuthenticationConfigurator : IConfigurator
    {
        public static readonly string authenticationCookie = "Authentication";

        private readonly IConfiguration configuration;

        public AuthenticationConfigurator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = this.configuration["Jwt:Issuer"],
                    ValidAudience = this.configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"])),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (c) =>
                    {
                        c.Token = c.HttpContext.Request.Cookies[authenticationCookie];
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
