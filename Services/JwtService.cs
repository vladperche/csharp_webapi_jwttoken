using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private SymmetricSecurityKey symmetricSecurityKey;
        private string _jwtToken;
        private ClaimsPrincipal _claims;

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

        public bool Validate(HttpRequest request)
        {
            request.Headers.TryGetValue("Authorization", out StringValues authorization);
            if (!authorization.Any())
            {
                return false;
            }

            var token = authorization
                .Where(c => c.StartsWith("Bearer "))
                .FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            _jwtToken = token["Bearer ".Length..];

            var tokenHandler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            try
            {
                _claims = tokenHandler.ValidateToken(_jwtToken, validations, out var tokenSecure);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public string ReadToken(string key)
        {
            var value = _claims.Claims
                .Where(c=> Object.Equals(c.Type, key))
                .Select(c => c.Value)
                .FirstOrDefault();
            return value;
        }
    }
}
