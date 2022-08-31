using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CrossCutting.Extensions
{
    public static class SecurityExtension
    {
        public static IServiceCollection AddSecurityProviders(this IServiceCollection services)
        {
            //Creates a Service Provider to get Dependency Injections without 'Constructor' parameters
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //Get's the JwtService that handles rules and manages Jwt issues
            IJwtService jwtService = serviceProvider.GetService<IJwtService>();
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = NegotiateDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddNegotiate()
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtService.GetSecurityKey(),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection AddSecurityPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            var enableOriginList = configuration.GetSection("EnabledOriginList").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder
                            .WithOrigins(enableOriginList)
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            return services;
        }
    }
}
