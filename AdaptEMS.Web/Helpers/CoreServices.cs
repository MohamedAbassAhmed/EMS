using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using AdaptEMS.Entities.SharedEntities.Account;
using AdaptEMS.Entities.SharedEntities.LeaveOrder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdaptEMS.Web.Helpers
{
    public class CoreServices
    {
        ApplicationDBContext _db;
        IConfiguration _configuration;
        IHttpContextAccessor _contextAccessor;
        public CoreServices(ApplicationDBContext db, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }
        public async Task<(bool Result, string Data)> PostAsync<T>(string url, T body)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["API_URL"]);
                var response = await client.PostAsJsonAsync<T>(url, body);
                if (!response.IsSuccessStatusCode)
                {
                    DBLog(Messages.LoginResponse, await response.Content.ReadAsStringAsync());
                    return (false, Messages.ExceptionOccured);
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<APIBaseResponse>();

                    if (!result.Success)
                    {
                        return (false, result.Message);
                    }
                    var options = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    return (true, JsonConvert.SerializeObject(result.Data, options));
                }
            }
        }
        public async Task<(bool Result, T Data)> GetAsync<T>(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["API_URL"]);
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    DBLog(Messages.LoginResponse, await response.Content.ReadAsStringAsync());
                    return (false, (T)Activator.CreateInstance<T>());
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<APIBaseResponse>();

                    if (!result.Success)
                    {
                        return (false, (T)Activator.CreateInstance<T>());
                    }
                    var options = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    var serilizedData = JsonConvert.SerializeObject(result.Data, options);
                    return (true, JsonConvert.DeserializeObject<T>(serilizedData));
                }
            }
        }
        public async Task<(bool Result, string Data)> GetAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["API_URL"]);
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    DBLog(Messages.LoginResponse, await response.Content.ReadAsStringAsync());
                    return (false, Messages.ExceptionOccured);
                }
                else
                {
                    var result = await response.Content.ReadAsAsync<APIBaseResponse>();

                    if (!result.Success)
                    {
                        return (false, (String)Activator.CreateInstance<String>());
                    }
                    var options = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    var serilizedData = JsonConvert.SerializeObject(result.Data, options);
                    return (true, JsonConvert.DeserializeObject<String>(serilizedData));
                }
            }
        }
        #region LeaveOrders
        public async Task<(bool Result, List<LeaveOrderListModel> Orders)> GetLeaveOrders()
        {
            var response = await PostAsync<LeaveOrdersCretiriaRequest>("api/LeaveOrder/GetLeaveOrders", new LeaveOrdersCretiriaRequest()
            {
                ApplicationUserId = "",
                Status = ""
            });
            var responseBody = JsonConvert.DeserializeObject<List<LeaveOrderListModel>>(response.Data);

            return (response.Result, responseBody);
        }
        public async Task<(bool Result, string Message)> ApproveOrder(int orderId)
        {
            return await GetAsync("api/LeaveOrder/ApproveOrder?orderId=" + orderId);
        }
        public async Task<(bool Result, string Message)> RejectOrder(int orderId)
        {
            return await GetAsync("api/LeaveOrder/RejectOrder?orderId=" + orderId);
        }
        public async Task<(bool Result, string Message)> CreateLeaveOrder(CreateLeaveOrderRequest model)
        {
            var response = await PostAsync<CreateLeaveOrderRequest>("api/LeaveOrders/CreateLeaveOrder", model);
            if (!response.Result)
            {
                return (false, response.Data);
            }
            return (true, "");
        }
        #endregion

        #region Account



        public async Task<(bool Result, string Message)> Login(LoginRequest model)
        {
            //model.Password = "Ad@ptME2023!@#";
            try
            {
                var response = await PostAsync<LoginRequest>("api/account/login", model);
                if (!response.Result)
                {
                    return (false, response.Data);
                }
                var responseBody = JsonConvert.DeserializeObject<LoginResponse>(response.Data);
                var claims = new List<Claim>{
                        new Claim(Consts.UserIDClaimName,responseBody.UserId),
                        new Claim(Consts.UserNameClaimName,responseBody.UserName),
                        new Claim(Consts.TokenClaimName,responseBody.Token),
                        new Claim(Consts.RoleClaimName,responseBody.Roles),
                    };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {

                };

                await _contextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, Messages.ExceptionOccured);
            }
        }
        public async Task<(bool Result, string Message)> Register(EmployeeRegisterRequest model)
        {
            var response = await PostAsync<EmployeeRegisterRequest>("api/account/register", model);
            if (!response.Result)
            {
                return (false, response.Data);
            }
            return (true, "");
        }

        public async Task<(bool Result, List<EmployeesListModel> Employees)> GetEmployees()
        {
            var response = await GetAsync<List<EmployeesListModel>>("api/account/GetEmployees");
            return response;
        }
        public async Task<(bool Result, UpdateEmployeeRequest Employee)> GetEmployee(string userId)
        {
            var response = await GetAsync<UpdateEmployeeRequest>("api/account/GetEmployee?userId=" + userId);
            return response;
        }
        public async Task<(bool Result, List<SelectListItem> Positions)> GetPositions()
        {
            var response = await GetAsync<List<PositionListModel>>("api/account/GetPositions");

            return (true, response.Data.Select(p => new SelectListItem(p.Name, p.Id + "")).ToList());
        }
        public async Task<(bool Result, string Message)> CreateEmployee(CreateEmployeeRequest model)
        {
            var response = await PostAsync<CreateEmployeeRequest>("/api/account/CreateEmployee", model);
            if (!response.Result)
            {
                return (false, response.Data);
            }
            return (true, "");
        }
        public async Task<(bool Result, string Message)> UpdateteEmployee(UpdateEmployeeRequest model)
        {
            model.Password = "";
            var response = await PostAsync<UpdateEmployeeRequest>("/api/account/UpdateEmployee", model);
            if (!response.Result)
            {
                return (false, response.Data);
            }
            return (true, "");
        }
        public async Task<(bool Result, string Message)> DeleteEmployee(string userId)
        {
            return await GetAsync("api/account/DeleteUser?userId=" + userId);

        }
        public async Task Logout()
        {
            await _contextAccessor.HttpContext.SignOutAsync();
        }
        #endregion
        public void DBLog(string action, string description)
        {
            _db.EMSLogs.Add(new EMSLog()
            {
                Category = Consts.APICallResponseLogCategory,
                Message = description,
                Transaction = action
            });
            _db.SaveChanges();
        }
    }
}
