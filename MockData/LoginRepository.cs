using Domain.Entities;
using Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;

namespace MockData
{
    public class LoginRepository : ILoginRepository
    {
        private List<LoginModel> logins = new List<LoginModel>
        {
            new LoginModel
            {
                Username = "admin",
                Password = "admin#123",
                FullName = "Administrator",
                Email = "admin@domain.com",
                Role = "Administrator"
            },
            new LoginModel
            {
                Username = "support",
                Password = "support#123",
                FullName = "System Support",
                Email = "support@domain.com",
                Role = "Support"
            },
            new LoginModel
            {
                Username = "user",
                Password = "user#123",
                FullName = "System User",
                Email = "user@domain.com",
                Role = "User",
                Profiles = new[]
                {
                    "teacher"
                }
            }
        };

        public IEnumerable<LoginModel> GetList() => logins;
    }
}
