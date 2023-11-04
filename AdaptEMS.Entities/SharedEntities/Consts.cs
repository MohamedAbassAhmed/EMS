using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities
{
    public class Consts
    {
        public enum AdminNotificationType
        {
            error,
            success,
            warning,
            info
        }
        #region LogCategories
        public const string DefaultLogCategory = "Default";
        public const string APICallResponseLogCategory = "APICallResponse";

        #endregion

        public const int GMT_To_UAE_Timing = 4;
        public const string PhoneRegex = @"^(\+?\d{1,3}[^0]|\d{1,4}\d{1,4}[^0])\d{9}$";
        public const string UserIDClaimName = "UserId";
        public const string UserNameClaimName = "UserName";
        public const string TokenClaimName = "Token";
        public const string RoleClaimName = "Role";
        #region LeaveOrdersStatuses
        public const string NewLeaveOrder = "New";
        public const string RejectedLeaveOrder = "Rejected";
        public const string ApprovedLeaveOrder = "Approved";

        #endregion

        #region AccountTypes
        public const string EmployeeAccountType = "Employee";
        public const string AdminAccountType = "Admin";

        #endregion
    }
}
