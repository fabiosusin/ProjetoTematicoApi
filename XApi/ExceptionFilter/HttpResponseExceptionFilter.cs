using DAO.DBConnection;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using DAO.General.Log;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;

namespace XApi.ExceptionFilter
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public HttpResponseExceptionFilter(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
        }

        public int Order { get; } = int.MaxValue - 10;
        protected LogHistoryDAO LogHistoryDAO;
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = context.Exception.Message,
                    StackTrace = context.Exception.StackTrace,
                    ExceptionMessage = context.Exception.InnerException?.Message,
                    Type = AppLogTypeEnum.XApiExceptionError,
                    Date = DateTime.Now
                });

            }
        }
    }
}
