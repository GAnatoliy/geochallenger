using Autofac;
using Mehdime.Entity;


namespace GeoChallenger.DIModules
{
    public class DataAccessModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register database.
            builder.RegisterType<DbContextScopeFactory>()
                .As<IDbContextScopeFactory>();
        }
    }
}