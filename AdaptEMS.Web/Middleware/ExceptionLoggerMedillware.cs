﻿using AdaptEMS.Entities.DBEntities;
using AdaptEMS.Entities.SharedEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptEMS.Web.Midellwares
{
    public class ExceptionLoggerMedillware : IExceptionFilter
    {
        ApplicationDBContext _db;
        public ExceptionLoggerMedillware(ApplicationDBContext db)
        {
            _db = db;
        }
        public void OnException(ExceptionContext context)
        {
            try
            {
                _db.EMSLogs.Add(new EMSLog()
                {
                    StackTrace = context.Exception.Message,
                    InnerException = context.Exception.InnerException.Message,
                    Message = context.Exception.Message,
                    Time = DateTime.UtcNow.AddHours(Consts.GMT_To_UAE_Timing),
                    Transaction = new JavaScriptSerializer().Serialize(context.HttpContext.Request)
                });
                _db.SaveChanges();
                context.Result = new ObjectResult(new APIBaseResponse()
                {
                    Message = Messages.ExceptionOccured
                })
                {
                    StatusCode = 200,

                };
                context.ExceptionHandled = true;
            }
            catch (Exception ex)
            {
                context.Result = new ObjectResult(new APIBaseResponse()
                {
                    Message = Messages.ExceptionOccured
                })
                {
                    StatusCode = 500,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
