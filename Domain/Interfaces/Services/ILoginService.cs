using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface ILoginService
    {
        AccessToken GetToken(LoginModel login, bool rememberMe);
    }
}
