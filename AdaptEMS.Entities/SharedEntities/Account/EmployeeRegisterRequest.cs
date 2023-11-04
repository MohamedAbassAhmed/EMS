using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities.Account
{
    public class EmployeeRegisterRequest
    {
        [EmailAddress]
        [Required]
        public string UserName { get; set; }
        [MinLength(4)]
        [Required]
        public string FullName { get; set; }
        [RegularExpression(Consts.PhoneRegex)]
        [Required]
        public string Phone { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}
