using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using AdaptEMS.Entities.SharedEntities.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptEMS.Web.Controllers
{
    public class AccountController : WebBaseController
    {
        public AccountController(ApplicationDBContext db, IConfiguration configurations, IHttpContextAccessor contextAccessor) : base(db, configurations, contextAccessor)
        {
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var response = await CS.Login(model);
            if (!response.Result)
            {
                Alert(response.Message, Consts.AdminNotificationType.error);
                return View(model);
            }
            return Redirect("/");
        }
        public async Task<IActionResult> Logout()
        {
            await CS.Logout();
            return Redirect("/account/login");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(EmployeeRegisterRequest model)
        {
            var response = await CS.Register(model);
            if (!response.Result)
            {
                Alert(response.Message, Consts.AdminNotificationType.error);
                return View(model);
            }
            Alert(Messages.Done, Consts.AdminNotificationType.success);

            return Redirect("/account/login");
        }
        public async Task<IActionResult> GetEmployees()
        {
            var response=await CS.GetEmployees();
            if (!response.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            return View(response.Employees);
        }
        public async Task<IActionResult> CreateEmployee()
        {
            var getPositions = await CS.GetPositions();
            if (!getPositions.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            ViewData["Positions"] = getPositions.Positions;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest model)
        {
            var response = await CS.CreateEmployee(model);
            if (!response.Result)
            {
                Alert(response.Message, Consts.AdminNotificationType.error);
                var getPositions = await CS.GetPositions();
                if (!getPositions.Result)
                {
                    Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                }
                ViewData["Positions"] = getPositions.Positions;
                return View(model);
            }
            return RedirectToAction(nameof(GetEmployees));
        }
        public async Task<IActionResult> UpdateEmployee(string userId)
        {
            var getPositions = await CS.GetPositions();
            if (!getPositions.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
            }
            ViewData["Positions"] = getPositions.Positions;
            var getEmployee = await CS.GetEmployee(userId);
            if (!getEmployee.Result)
            {
                Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                return RedirectToAction(nameof(GetEmployees));
            }
            return View(getEmployee.Employee);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeRequest model)
        {
            var response = await CS.UpdateteEmployee(model);
            if (!response.Result)
            {
                Alert(response.Message, Consts.AdminNotificationType.error);
                var getPositions = await CS.GetPositions();
                if (!getPositions.Result)
                {
                    Alert(Messages.ExceptionOccured, Consts.AdminNotificationType.error);
                }
                ViewData["Positions"] = getPositions.Positions;
                return View(model);
            }
            return RedirectToAction(nameof(GetEmployees));
        }
        public async Task<IActionResult> DeleteEmployee(string userId)
        {
            var response = await CS.DeleteEmployee(userId);
            if (!response.Result)
            {
                Alert(response.Message, Consts.AdminNotificationType.error);
            }
            return RedirectToAction(nameof(GetEmployees));

        }
    }
}
