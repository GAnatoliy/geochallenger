using System.Threading.Tasks;
using Owin;

namespace GeoChallenger.Web.Api
{
    public class RouteConfig
    {
        public static void Register(IAppBuilder app)
        {
            const int redirectStatusCode = 301;

            // For help page
            app.UseHandlerAsync((request, response, next) => {
                if (request.Path == "/") {
                    response.StatusCode = redirectStatusCode;
                    response.SetHeader("Location", "/Help");
                    return Task.Run(() => { });
                }

                return next();
            });
            // app.Map - using a regex exclude list would be better here so it doesn't fire for every request
            app.Map("/help", appbuilder => app.UseHandlerAsync((request, response, next) => next()));
        }
    }
}