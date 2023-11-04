using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.SharedEntities.Account
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Roles { get; set; }
    }
}
