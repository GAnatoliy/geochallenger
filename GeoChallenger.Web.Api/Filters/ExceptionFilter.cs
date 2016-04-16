using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using NLog;

namespace GeoChallenger.Web.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _log = LogManager.GetCurrentClassLogger();

        public bool AllowMultiple { get; }

        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            _log.Fatal(actionExecutedContext.Exception);

            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError) {
                Content = new ObjectContent(typeof(object), new {
                    errorDescription = actionExecutedContext.Exception.Message
                }, new JsonMediaTypeFormatter())
            };
            return Task.FromResult(string.Empty);
        }
    }
}