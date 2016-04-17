using Autofac;
using AutoMapper;
using GeoChallenger.Commands.Commands;
using GeoChallenger.DIModules;


namespace GeoChallenger.Commands.Command
{
    public class DIConfig
    {
        public static IContainer RegisterDI(MapperConfiguration mapperConfiguration)
        {
            var builder = new ContainerBuilder();

            // Register automapper.
            builder.Register(ctx => mapperConfiguration);
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            // Register modules.
            builder.RegisterModule(new DataAccessModule());
            builder.RegisterModule(new BusinessLogicModule());

            // Register commands.
            builder.RegisterType<ReindexCommand>();

            // Build the container
            return builder.Build();   
        }
    }
}