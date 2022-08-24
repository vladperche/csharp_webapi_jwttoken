using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;

namespace Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private SymmetricSecurityKey symmetricSecurityKey;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SymmetricSecurityKey GetSecurityKey()
        {
            if(symmetricSecurityKey == null)
            {
                var securityKey = Environment.GetEnvironmentVariable("JWT_SECURITY_KEY");
                if (string.IsNullOrEmpty(securityKey))
                    securityKey = _configuration["Jwt:SymmetricSecurityKey"];
                var key = Encoding.ASCII.GetBytes(securityKey);
                symmetricSecurityKey = new SymmetricSecurityKey(key);
            }

            return symmetricSecurityKey;
        }

        public DateTime ExpiresIn(bool rememberMe)
        {
            string value;
            if (rememberMe)
                value = _configuration["Jwt:ExpiresInRememberMe"];
            else
                value = _configuration["Jwt:ExpiresIn"];

            if (!int.TryParse(value, out int hours))
                hours = 8;

            return DateTime.UtcNow.AddHours(hours);
        }
    }
}
