using AdaptEMS.API.Helpers;
using AdaptEMS.Entities.DBEntities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptEMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIBaseController : ControllerBase
    {
        protected CoreServices CS;
        protected Validators validators;
        protected IConfiguration _configurations;

        public APIBaseController(ApplicationDBContext db,IConfiguration configuration,UserManager<ApplicationUser>userManager,
            SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            CS = new CoreServices(db,userManager,signInManager,configuration, roleManager);
            validators = new Validators(db, userManager);
            _configurations = configuration;
        }
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("Test")]
        public IActionResult Test()
        {
            //CS.initAsync();
            return Ok();
            
        }
    }
}
