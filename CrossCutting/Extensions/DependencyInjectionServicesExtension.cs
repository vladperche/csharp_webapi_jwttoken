using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Domain.Interfaces.Services;
using Services;
using Domain.Interfaces.Repositories;
using MockData;

namespace CrossCutting.Extensions
{
    public static class DependencyInjectionServicesExtension
    {
        public static IServiceCollection AddInjectionServices(this IServiceCollection services)
        {
            services
                .AddScoped<IJwtService, JwtService>()
                .AddScoped<ILoginRepository, LoginRepository>()
                .AddScoped<ILoginService, LoginService>();

            return services;
        }
    }
}
