using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoChallenger.Search.Documents;
using GeoChallenger.Services.Interfaces.DTO;
using Nest;


namespace GeoChallenger.Search.Providers
{
    public class PoisSearchProvider: DocumentsSearchProvider<PoiDocument>, IPoisSearchProvider
    {
        public PoisSearchProvider(SearchSettings searchSettings) : base(searchSettings)
        {
        }

        public async Task<IList<PoiDocument>> SearchAllAsync(string searchText = null, GeoBoundingBoxDto geoBoundingBox = null)
        {
            var client = GetClient();

            // TODO: check how to run one of two queries.
            var result = await client.SearchAsync<PoiDocument>(s => s
              // Use filter since for geo location isn't important result order.
             .Query(q => +q
                 .MultiMatch(m => m
                     .Query(searchText)
                     .Type(TextQueryType.BestFields)
                     .Fields(f => f.Field(p => p.Title).Field(p => p.Content))
                     .TieBreaker(0.3)
                     .MinimumShouldMatch("75%"))
                 && +q.GeoBoundingBox(b => b
                    .BoundingBox(geoBoundingBox.TopLeftLatitude, geoBoundingBox.TopLeftLongitude, geoBoundingBox.BottomRightLatitude, geoBoundingBox.BottomRightLongitude)
                    .Field(p => p.Location))));

            return result.Documents.ToList();
        }
    }
}