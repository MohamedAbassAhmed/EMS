using AdaptEMS.Entities.SharedEntities;
using AdaptEMS.Entities.SharedEntities.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.DBEntities
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; } = false;
        public string AccountType { get; set; } = "";
        public ApplicationUser()
        {

        }
        public ApplicationUser(EmployeeRegisterRequest model)
        {
            FullName = model.FullName;
            IsActive = true;
            AccountType = Consts.EmployeeAccountType;
            Email = model.UserName;
            UserName = model.UserName;
            PhoneNumber = model.Phone;
        }
    }
}
