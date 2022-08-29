using Domain.Entities.Response;
using Domain.Enums;
using WebAPI.CustomAuthorization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvironmentController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public EnvironmentController(
            IWebHostEnvironment env,
            IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        [HttpGet("Info")]
        [Authorize(Roles = RolesEnum.ADMINISTRATOR, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult Info()
        {
            try
            {
                var response = new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    StatusDescription = "Ok"
                };

                response.Messages.Add($"Application Name: {_env.ApplicationName}");
                response.Messages.Add($"Environment Name: {_env.EnvironmentName}");
                response.Messages.Add($"Content Root Path: {_env.ContentRootPath}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ResponseModel
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusDescription = ex.Message
                };
                return BadRequest(response);
            }
        }

        [HttpGet("Custom")]
        [ProfileAuthorization("teacher,secretary,director")]
        public ActionResult Custom()
        {
            return Ok("Ok");
        }
    }
}
