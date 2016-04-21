using System.Data.Entity.ModelConfiguration;
using GeoChallenger.Domains.Pois;


namespace GeoChallenger.Database.Config
{
    public class PoiEntityConfiguration: EntityTypeConfiguration<Poi>
    {
        public PoiEntityConfiguration()
        {

            // Poi has checkins made by users.
            HasMany(p => p.Checkins)
                .WithRequired(c => c.Poi)
                .HasForeignKey(c => c.PoiId);
        }
    }
}