using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities.Account
{
    public class LoginRequest
    {
        [MinLength(3)]
        [Required]
        public string UserName { get; set; }
        [MinLength(3)]
        [Required]
        public string Password { get; set; }
    }
}
