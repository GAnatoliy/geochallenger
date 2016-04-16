using System.Collections.Generic;
using System.Data.Entity;
using GeoChallenger.Database.Extensions;
using GeoChallenger.Domains.Pois;

namespace GeoChallenger.Database
{
    /// <summary>
    ///     Seed base data.
    /// </summary>
    public class GeoChallengerInitializer: DropCreateDatabaseAlways<GeoChallengerContext>
    {
        protected override void Seed(GeoChallengerContext context)
        {
            var pois = new List<Poi> {
                new Poi { PoiId = 1, Title = "Stub POI 1", Address = "Dobrovolskogo St, 1, Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.534159, 32.275574) },
                new Poi { PoiId = 2, Title = "Stub POI 2", Address = "Shevchenka St, 1, Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.515507, 32.262109) },
                new Poi { PoiId = 3, Title = "Stub POI 3", Address = "Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.500530, 32.232154) }
            };

            foreach (var poi in pois) {
                context.Pois.Add(poi);
            }

            base.Seed(context);
        }
    }
}