using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities.Account
{
    public class CreateEmployeeRequest : EmployeeRegisterRequest
    {
        public int PositionId { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
    }
}
