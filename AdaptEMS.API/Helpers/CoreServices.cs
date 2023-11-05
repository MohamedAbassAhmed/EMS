using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using AdaptEMS.Entities.SharedEntities.Account;
using AdaptEMS.Entities.SharedEntities.LeaveOrder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.API.Helpers
{
    public class CoreServices
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        IConfiguration _configuration;
        ApplicationDBContext _db;
        #region User


        public CoreServices(ApplicationDBContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _db = db;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        internal async Task initAsync()
        {
            var x = await _roleManager.CreateAsync(new IdentityRole(Consts.AdminAccountType));
            var y = await _roleManager.CreateAsync(new IdentityRole(Consts.EmployeeAccountType));

            var user = new ApplicationUser()
            {
                UserName = "admin@EMS.com",
                Email = "admin@EMS.com",
                AccountType = Consts.AdminAccountType
            ,
                IsActive = true,
                FullName = "admin@EMS.com",
                PhoneNumber = ""
            };
            var result = await _userManager.CreateAsync(user, "Ad@ptME2023!@#");

            var z = _userManager.AddToRoleAsync(user, Consts.AdminAccountType);
        }

        public async Task<(bool Result, string Message)> RegisterEmployee(EmployeeRegisterRequest model)
        {
            var identityResult = await _userManager.CreateAsync(new ApplicationUser(model), model.Password);
            if (!identityResult.Succeeded)
            {
                return (false, string.Join(",", identityResult.Errors.Select(e => e.Code)));
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == model.UserName);
            identityResult = await _userManager.AddToRoleAsync(user, Consts.EmployeeAccountType);
            return identityResult.Succeeded ? (true, "") : (false, string.Join(",", identityResult.Errors.Select(e => e.Code)));

        }
        public async Task<(bool Result, LoginResponse Response)> Login(LoginRequest model)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == model.UserName);
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
            if (!result.Succeeded)
            {
                return (false, new LoginResponse() { });
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var stringUserRoles = string.Join(',', userRoles);

            var tokenResult = await GenerateToken(user);
            return (true, new LoginResponse() { Token = tokenResult.Token, UserName = user.UserName, UserId = user.Id,Roles= stringUserRoles });
        }
        public async Task<(bool Result, string Token)> GenerateToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var stringUserRoles = string.Join(',', userRoles);

            var key = _configuration["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var ceredintial = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub,user.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, stringUserRoles),
                new Claim(Consts.UserIDClaimName,user.Id),
                new Claim(Consts.UserNameClaimName,user.UserName),
                new Claim(Consts.RoleClaimName,stringUserRoles),
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: ceredintial);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return (true, encodedToken);
        }
        public async Task<(bool Result, string Message)> CreateEmployee(CreateEmployeeRequest model)
        {
            var identityResult = await _userManager.CreateAsync(new ApplicationUser(model), model.Password);
            if (!identityResult.Succeeded)
                return (false, string.Join(",", identityResult.Errors));

            var user = _db.Users.FirstOrDefault(u => u.UserName == model.UserName);

            Employee employee = new Employee()
            {
                HireDate = DateTime.UtcNow.AddHours(Consts.GMT_To_UAE_Timing),
                Salary = model.Salary,
                PositionId = model.PositionId,
                ApplicationUserId = user.Id

            };
            _db.Employees.Add(employee);
            _db.SaveChanges();
            return (true, "");
        }
        public List<EmployeesListModel> GetEmployees()
        {
            var employees = (from user in _db.Users
                             join emp in _db.Employees on user.Id equals emp.ApplicationUserId into emps
                             from subpet in emps.DefaultIfEmpty()
                             where user.AccountType == Consts.EmployeeAccountType
                             select new EmployeesListModel()
                             {
                                 Position = subpet != null ? subpet.Position.Name : "",
                                 Salary = subpet != null ? subpet.Salary : 0,
                                 EmployeeId = subpet != null ? subpet.ID : 0,
                                 ApplicationUserId = user.Id,
                                 FullName = user.FullName,
                                 Phone = user.PhoneNumber,
                                 UserName = user.UserName,
                                 IsActive = user.IsActive

                             }).ToList();
            return employees;
        }
        public UpdateEmployeeRequest GetEmployee(string userId)
        {
            var user = _db.Users.Find(userId);
            var employee = _db.Employees.FirstOrDefault(e => e.ApplicationUserId == user.Id);
            var position = new Position();
            if (employee != null)
                position = _db.Positions.FirstOrDefault(e => e.Id == employee.PositionId);
            return new UpdateEmployeeRequest
            {
                Salary = employee != null ? employee.Salary : 0,
                HireDate = employee != null ? employee.HireDate : new DateTime(),
                PositionId = employee != null ? position.Id : 0,
                ApplicationUserId = userId,
                FullName = user.FullName,
                Phone = user.PhoneNumber,
                UserName = user.UserName,
            };
        }
        public List<PositionListModel> GetPositions()
        {
            var positions = _db.Positions.Where(p => p.IsActive).Select(p => new PositionListModel()
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            return positions;
        }
        public void UpdateEmployee(UpdateEmployeeRequest model)
        {
            var user = _db.Users.Find(model.ApplicationUserId);
            var employee = _db.Employees.FirstOrDefault(e => e.ApplicationUserId == model.ApplicationUserId);
            if (employee == null)
            {
                employee = new Employee()
                {
                    ApplicationUserId = model.ApplicationUserId
                };
            }
            employee.HireDate = model.HireDate;
            employee.Salary = model.Salary;
            employee.PositionId = model.PositionId;
            _db.Employees.Update(employee);
            _db.SaveChanges();
            user.UserName = model.UserName;
            user.PhoneNumber = model.Phone;
            user.Email = model.UserName;
            _userManager.UpdateAsync(user);

        }

        public void DeleteUser(string userId)
        {
            var user = _db.Users.Find(userId);
            var employee = _db.Employees.FirstOrDefault(e=>e.ApplicationUserId==userId);
            user.IsActive = false;
            _db.Users.Update(user);
            _db.SaveChanges();
        }
        #endregion

        #region LeaveOrders
        public void CreateLeaveOrder(CreateLeaveOrderRequest model, string userId)
        {
            var employee = _db.Employees.FirstOrDefault(e => e.ApplicationUserId == userId);
            LeaveOrder order = new LeaveOrder()
            {
                Status = Consts.NewLeaveOrder,
                CreateDate = DateTime.UtcNow.AddHours(Consts.GMT_To_UAE_Timing),
                Comment = model.Comment,
                RequestedLeaveDate = model.RequestedLeaveDate,
                EmployeeId = employee.ID,
            };
            _db.LeaveOrders.Add(order);
            _db.SaveChanges();
        }
        public void ApproveLeaveOrder(int orderId)
        {
            var order = _db.LeaveOrders.FirstOrDefault(o => o.Id == orderId);
            order.UpdateDate = DateTime.UtcNow.AddHours(Consts.GMT_To_UAE_Timing);
            order.Status = Consts.ApprovedLeaveOrder;
            _db.LeaveOrders.Update(order);
            _db.SaveChanges();

        }
        public void RejectLeaveOrder(int orderId)
        {
            var order = _db.LeaveOrders.FirstOrDefault(o => o.Id == orderId);
            order.UpdateDate = DateTime.UtcNow.AddHours(Consts.GMT_To_UAE_Timing);
            order.Status = Consts.RejectedLeaveOrder;
            _db.LeaveOrders.Update(order);
            _db.SaveChanges();

        }

        public List<LeaveOrderListModel> GetLeaveOrders(LeaveOrdersCretiriaRequest model)
        {

            var leaveOrders = new List<LeaveOrderListModel>();
            if (model.ApplicationUserId != "")
            {
                var employee = _db.Employees.FirstOrDefault(e => e.ApplicationUserId == model.ApplicationUserId);
                leaveOrders = _db.LeaveOrders.Where(o => o.EmployeeId == employee.ID)
                    .Include(o => o.Employee).ThenInclude(o => o.ApplicationUser)
                    .Select(o => new LeaveOrderListModel()
                    {
                        Id = o.Id,
                        Status = o.Status,
                        EmployeeFullName = o.Employee == null ? "" : o.Employee.ApplicationUser.FullName,
                        EmployeeUserName = o.Employee == null ? "" : o.Employee.ApplicationUser.FullName,
                        Comment = o.Comment,
                        RequestedLeaveDate = o.RequestedLeaveDate.ToString("dd-MM-yyyy")
                    }).ToList();
            }
            else
            {
                leaveOrders = _db.LeaveOrders.Include(o => o.Employee).ThenInclude(o => o.ApplicationUser)
                    .Select(o => new LeaveOrderListModel()
                    {
                        Id=o.Id,
                        Status = o.Status,
                        EmployeeFullName = o.Employee == null ? "" : o.Employee.ApplicationUser.FullName,
                        EmployeeUserName = o.Employee == null ? "" : o.Employee.ApplicationUser.FullName,
                        Comment = o.Comment,
                        RequestedLeaveDate = o.RequestedLeaveDate.ToString("dd-MM-yyyy")
                    }).ToList();
            }
            if (model.Status != "")
            {
                leaveOrders = leaveOrders.Where(o => o.Status == model.Status).ToList();
            }
            return leaveOrders;
        }
        #endregion
    }
}
