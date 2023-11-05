using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities
{
    public class Messages
    {
        public const string ExceptionOccured = "ExceptionOccured";
        public const string Done = "Done";

        #region User
        public const string UserNameAlreadyExist = "UserNameAlreadyExist";
        public const string NotValidUserToLogin = "NotValidUserToLogin";
        public const string NotValidLoginAttenmpts = "NotValidLoginAttenmpts";
        public const string NotValidPosition = "NotValidPosition";
        public const string NotValidEmployee = "NotValidEmployee";
        public const string YouHavePendingRequest = "YouHavePendingRequest";

        #endregion
        #region LeaveOrders
        public const string NotValidOrderToApprove = "NotValidOrderToApprove";
        public const string NotValidOrderToReject = "NotValidOrderToReject";

        #endregion

        #region WebActions
        public const string LoginResponse = "LoginResponse";
        #endregion
    }
}
