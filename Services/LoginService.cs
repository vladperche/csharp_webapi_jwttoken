using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;

using Domain.Entities;
using Domain.Entities.Request;
using Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;

namespace Services
{
    public class LoginService : ILoginService
    {
        private readonly IJwtService _jwtService;

        public LoginService(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public AccessToken GetToken(LoginModel login, bool rememberMe)
        {
            var expires = _jwtService.ExpiresIn(rememberMe);
            var claims = new Dictionary<string, object>();
            claims.Add("ExtraAttrib1", "ExtraValue1");
            claims.Add("ExtraAttrib2", "ExtraValue2");
            claims.Add("ExtraAttrib3", "ExtraValue3");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, login.Username),
                    new Claim(ClaimTypes.Name, login.FullName),
                    new Claim(ClaimTypes.Email, login.Email),
                    new Claim(ClaimTypes.Role, login.Role)
                }),
                NotBefore = DateTime.UtcNow,
                Expires = expires,
                SigningCredentials = new SigningCredentials(_jwtService.GetSecurityKey(), SecurityAlgorithms.HmacSha256Signature),
                Claims = claims
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            var accessToken = new AccessToken
            {
                Token = token,
                Username = login.Username,
                FullName = login.FullName,
                Email = login.Email,
                Expires = expires
            };

            return accessToken;
        }
    }
}
