using System.Collections.Generic;
using Autofac;
using GeoChallenger.Search;
using GeoChallenger.Search.Providers;
using GeoChallenger.Services;
using GeoChallenger.Services.Core;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Users;
using GeoChallenger.Services.Providers;
using GeoChallenger.Services.Providers.Interfaces;
using GeoChallenger.Services.Providers.SocialNetworks;
using GeoChallenger.Services.Settings;


namespace GeoChallenger.DIModules
{
    public class BusinessLogicModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register settings
            builder.RegisterInstance(SettingsFactory.GetFacebookSettings());
            builder.RegisterInstance(SettingsFactory.GetGoogleSettings());
            builder.RegisterInstance(SettingsFactory.GetAuthenticationSettings());
            builder.RegisterInstance(new SearchSettings());

            // Register services.
            var assembly = typeof(PoisService).Assembly;
            builder.RegisterAssemblyTypes(assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();
            // TODO: check possible issue with IIS and decide if it is actual for our case
            // http://docs.autofac.org/en/latest/register/scanning.html#iis-hosted-web-applications

            // Register http provider
            builder.RegisterType<GeoChallengerHttpProvider>()
                .As<IHttpProvider>();

            // Social network providers
            builder.RegisterType<FacebookProvider>().AsSelf();

            builder.RegisterType<GoogleProvider>().AsSelf();

            // Register social provides according to social account type
            builder.Register(context => new Dictionary<AccountTypeDto, ISocialNetworksProvider> {
                { AccountTypeDto.Facebook, context.Resolve<FacebookProvider>() },
                { AccountTypeDto.Google, context.Resolve<GoogleProvider>() }
            });

            // Register search.
            builder.RegisterType<SearchConfigurationManager>()
                .As<ISearchConfigurationManager>();

            builder.RegisterType<PoisSearchProvider>()
                .As<IPoisSearchProvider>();

            builder.RegisterType<SearchIndexer>()
                .As<ISearchIndexer>();

            // Register commands.
            builder.RegisterType<Commands>()
                .As<ICommands>();
            
        }
    }
}