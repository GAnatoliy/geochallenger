using System.Collections.Generic;
using System.Data.Entity;
using GeoChallenger.Domains;


namespace GeoChallenger.Database
{
    /// <summary>
    /// Seed base data.
    /// </summary>
    public class GeoChallengerInitializer: DropCreateDatabaseIfModelChanges<GeoChallengerContext>
    {
        protected override void Seed(GeoChallengerContext context)
        {
            var pois = new List<Poi> {
                new Poi { PoiId = 1, Title = "Stub POI 1" },
                new Poi { PoiId = 2, Title = "Stub POI 2" },
                new Poi { PoiId = 3, Title = "Stub POI 3" }
            };

            foreach (var poi in pois) {
                context.Pois.Add(poi);
            }

            base.Seed(context);
        }
    }
}