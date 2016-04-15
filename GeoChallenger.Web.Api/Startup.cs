using System.Web.Http;
using System.Web.Mvc;
using GeoChallenger.Web.Api;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace GeoChallenger.Web.Api
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration;

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration = new HttpConfiguration();

            var mapperConfiguration = MapperConfig.CreateMapperConfiguration();

            DIConfig.RegisterDI(HttpConfiguration, mapperConfiguration);

            WebApiConfig.Register(app, HttpConfiguration);

            AreaRegistration.RegisterAllAreas();

            RouteConfig.Register(app);

            // NOTE: Don't put code below this line
            HttpConfiguration.EnsureInitialized();
        }
    }
}