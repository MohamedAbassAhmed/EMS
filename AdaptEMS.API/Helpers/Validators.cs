using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using AdaptEMS.Entities.SharedEntities.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.API.Helpers
{
    public class Validators
    {
        UserManager<ApplicationUser> _userManager;
        ApplicationDBContext _db;

        public Validators(ApplicationDBContext db, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }
        #region User
        public bool IsValidUserToLogin(string userName)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName && u.IsActive);
            return user is not null;
        }
        public bool IsValidUserTobeRegistered(EmployeeRegisterRequest model)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == model.UserName );
            return user is null;
        }
        public (bool Result,string Message) IsValidUserToCreate(CreateEmployeeRequest model)
        {
            StringBuilder message = new StringBuilder();
            if (!IsValidUserTobeRegistered(model))
            {
                message.Append(Messages.UserNameAlreadyExist);
            }
            if (!IsValidPosition(model.PositionId))
            {
                message.Append((",")+Messages.NotValidPosition);

            }
            return (message.Length==0, message.ToString());
        }
        public bool IsValidPosition(int positionID)
        {
            var position = _db.Positions.FirstOrDefault(p=>p.Id==positionID&&p.IsActive);
            return position is not null;
        }

        public (bool Result,string Message)IsValidDataToUpdateEmployee(UpdateEmployeeRequest model)
        {
            if (!IsValidEmployee(model.ApplicationUserId))
            {
                return (false,Messages.NotValidEmployee);
            }
            StringBuilder message = new StringBuilder();

            var user = _db.Users.Find(model.ApplicationUserId);
            var employee = _db.Employees.FirstOrDefault(e=>e.ApplicationUserId==model.ApplicationUserId);
            if (!IsValidUserNameToUpdateWith(model.UserName, user.Id))
            {
                message.Append( Messages.UserNameAlreadyExist);
            }
            if (!IsValidPosition(model.PositionId))
            {
                message.Append((",") + Messages.NotValidPosition);
            }
            return (message.Length==0, message.ToString());
        }
        public bool IsValidEmployee(int employeeId)
        {
            var employee = _db.Employees.Find(employeeId);
            return employee is not null;
        }
        public bool IsValidEmployee(string applicationUserId)
        {
            var employee = _db.Users.FirstOrDefault(e=>e.Id== applicationUserId&&e.AccountType==Consts.EmployeeAccountType);
            return employee is not null;
        }
        public bool IsValidUserNameToUpdateWith(string userName,string userId)
        {
            var oldUser = _db.Users.FirstOrDefault(u=>u.UserName==userName&&u.Id!=userId);
            return oldUser is null;
        }
        #endregion

        #region LeaveOrder
        public bool IsValidEmployeeToOrderLeave(string userId)
        {
            var employee = _db.Employees.FirstOrDefault(e=>e.ApplicationUserId==userId);
            return employee is not null;
        }
        public bool EmployeeDoseNotHavePendingOrder(string userId)
        {
            var employee = _db.Employees.FirstOrDefault(e => e.ApplicationUserId == userId);
            var penddingLeaveOrders = _db.LeaveOrders.FirstOrDefault(o => o.EmployeeId == employee.ID && o.Status == Consts.NewLeaveOrder);
            return penddingLeaveOrders is null;
        }
        public bool IsValidLeaveOrderToApprove(int orderId)
        {
            var order = _db.LeaveOrders.FirstOrDefault(o=>o.Id==orderId&&o.Status==Consts.NewLeaveOrder);
            return order is not null;
        }
        public bool IsValidLeaveOrderToReject(int orderId)
        {
            var order = _db.LeaveOrders.FirstOrDefault(o => o.Id == orderId && (o.Status == Consts.NewLeaveOrder ));
            return order is not null;
        }
        #endregion
    }
}
