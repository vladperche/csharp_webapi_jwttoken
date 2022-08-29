using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebAPI.CustomAuthorization
{
    /// <summary>
    /// This class implements a custom Authorization for JWT Tokens
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ProfileAuthorization : Attribute, IAuthorizationFilter
    {
        private readonly string[] _profiles;

        /// <summary>
        /// The constructor will receive the parameters defined in the Endpoint annotation.
        /// For example:
        /// [ProfileAuthorization("profile1,profile2,profileN")]
        /// 
        /// And will convert this string, to a string[] splited by ','
        /// </summary>
        /// <param name="profiles"></param>
        public ProfileAuthorization(string profiles)
        {
            //Authorized profiles;
            _profiles = profiles.Split(',');
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtService = (IJwtService)context.HttpContext.RequestServices.GetService(typeof(IJwtService));
            var loginRepository = (ILoginRepository)context.HttpContext.RequestServices.GetService(typeof(ILoginRepository));

            //Validates the JWT Token in the Authorization Header
            if (!jwtService.Validate(context.HttpContext.Request))
            {
                context.Result = new ForbidResult();
                return;
            }

            //Lookup for the username
            var username = jwtService.ReadToken(ClaimTypes.NameIdentifier);
            var user = loginRepository.GetList().Where(u => u.Username.Equals(username)).FirstOrDefault();
            if(user == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            //Lookup for user profiles
            var userClaims = user?.Profiles
                ?.Where(c => _profiles.Any(x => x.Equals(c)))
                ?.ToList();

            //If none was found, return Forbid
            if (userClaims == null || !userClaims.Any())
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
