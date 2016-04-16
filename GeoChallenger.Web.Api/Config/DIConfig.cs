using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using GeoChallenger.Services;
using GeoChallenger.Web.Api.Filters;
using GeoChallenger.Web.Api.Providers;
using GeoChallenger.Web.Api.Providers.Interfaces;
using Mehdime.Entity;


namespace GeoChallenger.Web.Api.Config
{
    public class DIConfig
    {
        // Register all related DI.
        public static void RegisterDI(HttpConfiguration configuration, MapperConfiguration mapperConfiguration)
        {
            var builder = new ContainerBuilder();

            // Register database.
            builder.RegisterType<DbContextScopeFactory>()
                .As<IDbContextScopeFactory>();

            // Register automapper.
            builder.Register(ctx => mapperConfiguration);
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            // Register services.
            var assembly = typeof(PoisService).Assembly;
            builder.RegisterAssemblyTypes(assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();

            // Register OAuth authentication provider
            builder.RegisterType<GeoChallengerOAuthProvider>()
                .As<IGeoChallengerOAuthProvider>()
                .SingleInstance();  

            // TODO: check possible issue with IIS and decide if it is actual for our case
            // http://docs.autofac.org/en/latest/register/scanning.html#iis-hosted-web-applications

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Register filters
            configuration.Filters.Add(new ExceptionFilter());
        }
    }
}