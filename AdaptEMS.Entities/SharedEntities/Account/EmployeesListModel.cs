using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities.Account
{
    public class EmployeesListModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public int EmployeeId { get; set; }
        public string ApplicationUserId { get; set; }
        public bool IsActive { get; set; }
    }
}
