using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using AdaptEMS.Entities.SharedEntities.LeaveOrder;
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
    public class LeaveOrderController : APIBaseController
    {
        public LeaveOrderController(ApplicationDBContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager) 
            : base(db, configuration, userManager, signInManager,roleManager)
        {
        }
        [Authorize]
        [HttpPost("CreateLeaveOrder")]
        public IActionResult CreateLeaveOrder([FromBody] CreateLeaveOrderRequest model)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type.Equals(Consts.UserIDClaimName, StringComparison.InvariantCultureIgnoreCase)).Value;

            if (!validators.IsValidEmployeeToOrderLeave(userId))
            {
                return Ok(new APIBaseResponse()
                {
                    Message = Messages.NotValidEmployee
                });
            } if(!validators.EmployeeDoseNotHavePendingOrder(userId))
            {
                return Ok(new APIBaseResponse()
                {
                    Message = Messages.YouHavePendingRequest
                });
            }
            CS.CreateLeaveOrder(model, userId);
            return Ok(new APIBaseResponse()
            {
                Success = true
            });
        }
        [Authorize]
        [HttpGet("ApproveOrder")]
        public IActionResult ApproveOrder(int orderId)
        {
            if (!validators.IsValidLeaveOrderToApprove(orderId))
            {
                return Ok(new APIBaseResponse()
                {
                    Message = Messages.NotValidOrderToApprove
                });
            }
            CS.ApproveLeaveOrder(orderId);
            return Ok(new APIBaseResponse()
            {
                Success = true
            });
        }
        [HttpGet("RejectOrder")]
        public IActionResult RejectOrder(int orderId)
        {
            if (!validators.IsValidLeaveOrderToReject(orderId))
            {
                return Ok(new APIBaseResponse()
                {
                    Message = Messages.NotValidOrderToReject
                });
            }
            CS.ApproveLeaveOrder(orderId);
            return Ok(new APIBaseResponse()
            {
                Success = true
            });
        }
        [HttpPost("GetLeaveOrders")]
        public IActionResult GetLeaveOrders([FromBody]LeaveOrdersCretiriaRequest model)
        {
            if (model.ApplicationUserId != "" && !validators.IsValidEmployee(model.ApplicationUserId))
            {
                return Ok(new APIBaseResponse() { Message = Messages.NotValidEmployee });
            }
            var orders = CS.GetLeaveOrders(model);
            return Ok(new APIBaseResponse() { Data = orders, Success = true });
        }
    }
}
