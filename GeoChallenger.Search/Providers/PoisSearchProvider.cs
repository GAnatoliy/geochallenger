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

            var result = await client.SearchAsync<PoiDocument>(s => s
                // Use filter since for geo location isn't important result order.
                .Query(q => +q.Bool(bs => bs
                    .Must(mq => string.IsNullOrEmpty(searchText) ? mq : mq.MultiMatch(m => m
                        .Query(searchText)
                        .Type(TextQueryType.BestFields)
                        .Fields(f => f.Field(p => p.Title).Field(p => p.Content))
                        .TieBreaker(0.3)
                        .MinimumShouldMatch("75%")),
                        mq => geoBoundingBox == null ? mq : mq.GeoBoundingBox(b => b
                            .BoundingBox(geoBoundingBox.TopLeftLatitude, geoBoundingBox.TopLeftLongitude, geoBoundingBox.BottomRightLatitude, geoBoundingBox.BottomRightLongitude)
                            .Field(p => p.Location))))
                ));
            return result.Documents.ToList();
        }
    }
}