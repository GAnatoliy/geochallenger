using System.Data.Entity;
using GeoChallenger.Database.Config;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Routes;
using GeoChallenger.Domains.Users;

namespace GeoChallenger.Database
{
    public class GeoChallengerContext: DbContext
    {
        public GeoChallengerContext() : base("GeoChallengerDb")
        {
            System.Data.Entity.Database.SetInitializer(new GeoChallengerInitializer());
        }

        public DbSet<Poi> Pois { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Route> Routes { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserEntityConfiguration());
            modelBuilder.Configurations.Add(new RouteEntityConfiguration());
        }
    }
}