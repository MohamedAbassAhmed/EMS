using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities.LeaveOrder
{
    public class LeaveOrderListModel
    {
        public string EmployeeUserName { get; set; }
        public string EmployeeFullName { get; set; }
        public string RequestedLeaveDate { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public int Id { get; set; }

    }
}
