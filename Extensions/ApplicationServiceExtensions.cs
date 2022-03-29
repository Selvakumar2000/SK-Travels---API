using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SKTravelsApp.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKTravelsApp.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {         
            //For Jwt Token Creation and Handling
            services.AddScoped<TokenService>();
            //For Userprofile Management
            services.AddScoped<UserService>();
            //For Travels Management
            services.AddScoped<TravelsService>();

            return services;

        }
    }
}
