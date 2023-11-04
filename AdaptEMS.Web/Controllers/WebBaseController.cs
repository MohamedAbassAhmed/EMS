using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using AdaptEMS.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptEMS.Web.Controllers
{
    public class WebBaseController : Controller
    {
        protected ApplicationDBContext _db;
        protected IConfiguration _configurations;
        protected IHttpContextAccessor _contextAccessor;
        protected CoreServices CS;
        public WebBaseController(ApplicationDBContext db,IConfiguration configurations,IHttpContextAccessor contextAccessor)
        {
            _db = db;
            _configurations = configurations;
            _contextAccessor = contextAccessor;
            CS = new CoreServices(db, configurations, contextAccessor);
        }
        protected void Alert(string message, Consts.AdminNotificationType notificationType)
        {
            string msg = "";
            switch (notificationType)
            {
                case Consts.AdminNotificationType.success:
                    msg = " title='Done';message='" + @message + "';type='" + notificationType.ToString().ToLower() + "'; sweetAlert( title,message,type);";
                    break;
                case Consts.AdminNotificationType.error:
                    msg = " title='Error';message='" + @message + "';type='" + notificationType.ToString().ToLower() + "'; sweetAlert( title,message,type);";

                    break;
                case Consts.AdminNotificationType.info:
                    msg = " title='Alert';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; swal({title: title,text:message,type: type});";

                    break;
                case Consts.AdminNotificationType.warning:
                    msg = " title='Warning';message='" + message + "';type='" + notificationType.ToString().ToLower() + "'; swal({title: title,text:message,type: type});";

                    break;
            }
            TempData["notification"] = msg;
        }

       
    }
}
