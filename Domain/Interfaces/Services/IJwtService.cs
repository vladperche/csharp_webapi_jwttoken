using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IJwtService
    {
        SymmetricSecurityKey GetSecurityKey();
        DateTime ExpiresIn(bool rememberMe);
        public bool Validate(HttpRequest request);
        public string ReadToken(string key);
    }
}
