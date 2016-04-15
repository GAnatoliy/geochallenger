﻿using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using GeoChallenger.Services;
using GeoChallenger.Services.Interfaces;


namespace GeoChallenger.Web.Api
{
    public class DIConfig
    {
        // Register all related DI.
        public static void RegisterDI(MapperConfiguration mapperConfiguration)
        {
            var builder = new ContainerBuilder();

            // Register automapper.
            builder.Register(ctx => mapperConfiguration);
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            // Register services.
            var assembly = typeof(TagsService).Assembly;
            builder.RegisterAssemblyTypes(assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces();

            // TODO: check possible issue with IIS and decide if it is actual for our case
            // http://docs.autofac.org/en/latest/register/scanning.html#iis-hosted-web-applications

            var config = GlobalConfiguration.Configuration;
      
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);       
        }
    }
}
