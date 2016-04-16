using System.Data.Entity;
using GeoChallenger.Domains;


namespace GeoChallenger.Database
{
    public class GeoChallengerContext: DbContext
    {
        public DbSet<Poi> Pois { get; set; }
    }
}
