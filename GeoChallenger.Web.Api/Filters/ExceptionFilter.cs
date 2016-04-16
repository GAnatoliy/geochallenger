using System.Web.Http.Filters;
using NLog;

namespace GeoChallenger.Web.Api.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            _log.Error(filterContext.Exception, "Unhandled exception");
        }
    }
}