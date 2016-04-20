using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using GeoChallenger.Services.Interfaces.Exceptions;
using NLog;

namespace GeoChallenger.Web.Api.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            if (filterContext.Exception is BusinessLogicException) {
                _log.Warn(filterContext.Exception, "Unhandled exception");
            } else {
                _log.Error(filterContext.Exception, "Unhandled exception");
            }

            // We returns business logic exception as a part of BadRequest response, so we don't need to duplicate logic in 
            // presentation layer.
            if (filterContext.Exception is BusinessLogicException) {
                filterContext.Response =
                    filterContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, filterContext.Exception.Message);
            }
        }
    }
}