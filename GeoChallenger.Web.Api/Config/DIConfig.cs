using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using GeoChallenger.DIModules;
using GeoChallenger.Web.Api.Providers;


namespace GeoChallenger.Web.Api.Config
{
    public class DIConfig
    {
        // Register all related DI.
        public static void RegisterDI(HttpConfiguration configuration, MapperConfiguration mapperConfiguration)
        {
            var builder = new ContainerBuilder();

            // Register automapper.
            builder.Register(ctx => mapperConfiguration);
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            // Register modules.
            builder.RegisterModule(new DataAccessModule());
            builder.RegisterModule(new BusinessLogicModule());

            // Register OAuth authentication provider
            builder.RegisterType<GeoChallengerOAuthProvider>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}