using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using AdaptEMS.Entities.SharedEntities.LeaveOrder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptEMS.Web.Controllers
{
    public class LeaveOrdersController : WebBaseController
    {
        public LeaveOrdersController(ApplicationDBContext db, IConfiguration configurations, IHttpContextAccessor contextAccessor) : base(db, configurations, contextAccessor)
        {
        }

        public async Task<IActionResult> Index()
        {
            var response = await CS.GetLeaveOrders();
            if (!response.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(response.Orders);
        }
        public async Task<IActionResult> Approve(int orderId)
        {
            var response = await CS.ApproveOrder(orderId);
            if (!response.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Reject(int orderId)
        {
            var response = await CS.RejectOrder(orderId);
            if (!response.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Create(CreateLeaveOrderRequest model)
        {
            var response=await CS.CreateLeaveOrder(model);
            if (!response.Result)
            {
                Alert(response.Message, Consts.AdminNotificationType.error);
                return View(model);
            }
            else
            {
                Alert(Messages.Done, Consts.AdminNotificationType.success);
            }
            return View();
        }
        
    }
}
