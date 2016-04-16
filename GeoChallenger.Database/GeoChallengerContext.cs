using System.Data.Entity;
using GeoChallenger.Domains.Pois;


namespace GeoChallenger.Database
{
    public class GeoChallengerContext: DbContext
    {
        public GeoChallengerContext() : base("GeoChallengerDb")
        {
            System.Data.Entity.Database.SetInitializer(new GeoChallengerInitializer());
        }

        public DbSet<Poi> Pois { get; set; }
    }
}
