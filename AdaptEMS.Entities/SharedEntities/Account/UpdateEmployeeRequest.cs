using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities.Account
{
    public class UpdateEmployeeRequest: CreateEmployeeRequest
    {
        public string ApplicationUserId { get; set; }
        public string Password { get; set; }
    }
}
