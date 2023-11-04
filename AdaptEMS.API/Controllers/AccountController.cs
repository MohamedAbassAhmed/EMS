using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using AdaptEMS.Entities.SharedEntities.Account;
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
    public class AccountController : APIBaseController
    {
        public AccountController(ApplicationDBContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager) 
            : base(db, configuration, userManager, signInManager,roleManager)
        {
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest model)
        {
            if (!validators.IsValidUserToLogin(model.UserName))
            {
                return Ok(new APIBaseResponse()
                {
                    Message=Messages.NotValidUserToLogin,
                    Data = model.UserName
                });
            }
            var loginResult = await CS.Login(model);
            if (!loginResult.Result)
            {
                return Ok(new APIBaseResponse()
                {
                    Message = Messages.NotValidLoginAttenmpts
                });
            }
            return Ok(new APIBaseResponse()
            {
                Success=true,
                Data = loginResult.Response
            }) ;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]EmployeeRegisterRequest model)
        {
            if (!validators.IsValidUserTobeRegistered(model))
            {
                return Ok(new APIBaseResponse()
                {
                    Message = Messages.UserNameAlreadyExist
                });
            }
            var registerResult = await CS.RegisterEmployee(model);
            if (!registerResult.Result)
            {
                return Ok(new APIBaseResponse()
                {
                    Message = registerResult.Message
                });
            }
            return Ok(new APIBaseResponse()
            {
                Success = true
            });
        }
        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody]CreateEmployeeRequest model)
        {
            var isValidRequest = validators.IsValidUserToCreate(model);
            if (!isValidRequest.Result)
            {
                return Ok(new APIBaseResponse()
                {
                    Message = isValidRequest.Message
                });
            }
            var result =await CS.CreateEmployee(model);
            if (!result.Result)
            {
                return Ok(new APIBaseResponse()
                {
                    Message = result.Message
                });
            }
            return Ok(new APIBaseResponse()
            {
                Success = true
            });
        }
        [HttpGet("GetEmployees")]
        public IActionResult GetEmployees()
        {
            var getResult = CS.GetEmployees();
            return Ok(new APIBaseResponse()
            {
                Data = getResult,
                Success = true
            });
        }
        [HttpGet("GetEmployee")]
        public IActionResult GetEmployees(string userId)
        {
            var getResult = CS.GetEmployee(userId);
            return Ok(new APIBaseResponse()
            {
                Data = getResult,
                Success = true
            });
        }
        [HttpPost("UpdateEmployee")]
        public IActionResult UpdateEmployee([FromBody]UpdateEmployeeRequest model)
        {
            var isValid = validators.IsValidDataToUpdateEmployee(model);
            if (!isValid.Result)
            {
                return Ok(new APIBaseResponse()
                {
                    Message = isValid.Message
                });
            }
             CS.UpdateEmployee(model);
            return Ok(new APIBaseResponse()
            {
                Success = true
            }) ;
        }
        [HttpGet("DeleteUser")]
        public IActionResult DeleteUser(string userId)
        {
            if (!validators.IsValidEmployee(userId))
            {
                return Ok(new APIBaseResponse()
                {
                    Message = Messages.NotValidEmployee
                });
            }
            CS.DeleteUser(userId);
            return Ok(new APIBaseResponse()
            {
                Success = true
            });
        }
        [HttpGet("GetPositions")]
        public IActionResult GetPositions()
        {
            var getResult = CS.GetPositions();
            return Ok(new APIBaseResponse()
            {
                Data = getResult,
                Success = true
            });
        }
    }
}
