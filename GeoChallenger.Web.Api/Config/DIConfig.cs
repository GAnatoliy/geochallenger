using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using GeoChallenger.Services;
using GeoChallenger.Services.Interfaces.DTO.Users;
using GeoChallenger.Services.Providers;
using GeoChallenger.Services.Providers.Interfaces;
using GeoChallenger.Services.Providers.SocialNetworks;
using GeoChallenger.Services.Settings;
using GeoChallenger.Services.Settings.SocialNetworks;
using GeoChallenger.Web.Api.Filters;
using GeoChallenger.Web.Api.Providers;
using Mehdime.Entity;


namespace GeoChallenger.Web.Api.Config
{
    public class DIConfig
    {
        private static FacebookSettings _facebookSettings;
        private static GoogleSettings _googleSettings;

        public DIConfig()
        {
            _facebookSettings = SettingsFactory.GetFacebookSettings();
            _googleSettings = SettingsFactory.GetGoogleSettings();
        }

        // Register all related DI.
        public static void RegisterDI(HttpConfiguration configuration, MapperConfiguration mapperConfiguration)
        {
            var builder = new ContainerBuilder();

            // Register settings
            builder.RegisterInstance(_facebookSettings);
            builder.RegisterInstance(_googleSettings);

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
            
            // Register http provider
            builder.RegisterType<GeoChallengerHttpProvider>()
                .As<IHttpProvider>();

            // Register OAuth authentication provider
            builder.RegisterType<GeoChallengerOAuthProvider>()
                .AsSelf()
                .SingleInstance();

            // Social providers
            builder.RegisterType<FacebookProvider>().AsSelf();

            builder.RegisterType<GoogleProvider>().AsSelf();

            // Register social provides according to social account type
            builder.Register(context => new Dictionary<AccountTypeDto, ISocialNetworksProvider> {
                    { AccountTypeDto.Facebook, context.Resolve<FacebookProvider>() },
                    { AccountTypeDto.Google, context.Resolve<GoogleProvider>() }
                });

            // TODO: check possible issue with IIS and decide if it is actual for our case
            // http://docs.autofac.org/en/latest/register/scanning.html#iis-hosted-web-applications

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}