using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities.LeaveOrder
{
    public class CreateLeaveOrderRequest
    {
        public DateTime RequestedLeaveDate { get; set; }
        public string Comment { get; set; }
    }
}
