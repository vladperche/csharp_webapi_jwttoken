using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public static class RolesEnum
    {
        public const string ADMINISTRATOR = "Administrator";
        public const string SUPPORT = "Support";
        public const string USER = "User";

        public static readonly string ALL = string.Join(',',
            new[]
            {
                ADMINISTRATOR,
                SUPPORT,
                USER
            }
        );
    }
}
