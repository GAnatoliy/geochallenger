using System.Web.Http;
using System.Web.Mvc;
using GeoChallenger.Web.Api;
using GeoChallenger.Web.Api.Config;
using Microsoft.Owin;
using NLog;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace GeoChallenger.Web.Api
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            ILogger logger = LogManager.GetCurrentClassLogger();

            logger.Log(LogLevel.Info, "Started");

            HttpConfiguration = new HttpConfiguration();

            DIConfig.RegisterDI(HttpConfiguration, MapperConfig.CreateMapperConfiguration());

            OAuthConfig.ConfigureOAuth(app, HttpConfiguration);

            WebApiConfig.Register(app, HttpConfiguration);

            AreaRegistration.RegisterAllAreas();

            RouteConfig.Register(app);

            // NOTE: Don't put code below this line
            HttpConfiguration.EnsureInitialized();
        }
    }
}