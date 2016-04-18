using System.Data.Entity.ModelConfiguration;
using GeoChallenger.Domains.Routes;

namespace GeoChallenger.Database.Config
{
    public class RouteEntityConfiguration : EntityTypeConfiguration<Route>
    {
        public RouteEntityConfiguration()
        {
            // Many-to-many relationship (Routes-Pois)
            HasMany(r => r.Pois)
                .WithMany(p => p.Routes);
        }
    }
}