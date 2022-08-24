using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Domain.Entities.Request;
using Domain.Interfaces.Repositories;
using Domain.Entities.Response;
using Domain.Resources;
using Domain.Interfaces.Services;
using System.Net;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly ILoginService _loginService;

        public AuthController(
            ILoginRepository loginRepository,
            ILoginService loginService)
        {
            _loginRepository = loginRepository;
            _loginService = loginService;
        }

        /// <summary>
        /// Authenticate and get Token
        /// </summary>
        /// <param name="model">User information (username & password)</param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginRequestModel model)
        {
            try
            {
                var login = _loginRepository.GetList()
                    .Where(u => u.Username.Equals(model.Username) && u.Password.Equals(model.Password))
                    .FirstOrDefault();
                if(login == null)
                {
                    var notAuthorized = new ResponseModel
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        StatusDescription = Messages.Login_Forbbiden
                    };
                    return new ForbidResult(JsonConvert.SerializeObject(notAuthorized));
                }

                var accessToken = _loginService.GetToken(login, model.RememberMe);
                return Ok(accessToken);
            }
            catch (Exception ex)
            {
                var badRequest = new ResponseModel
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusDescription = ex.Message
                };
                badRequest.Messages.AddRange(ex.Source?.Split('\n'));
                return BadRequest(badRequest);
            }
        }
    }
}
