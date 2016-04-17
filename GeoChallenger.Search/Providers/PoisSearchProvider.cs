using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoChallenger.Search.Documents;
using Nest;


namespace GeoChallenger.Search.Providers
{
    public class PoisSearchProvider: DocumentsSearchProvider<PoiDocument>, IPoisSearchProvider
    {
        public PoisSearchProvider(SearchSettings searchSettings) : base(searchSettings)
        {
        }

        public async Task<IList<PoiDocument>> SearchAllAsync(string searchText)
        {
            var client = GetClient();

            var result = await client.SearchAsync<PoiDocument>(s => s
                // Use filter since for geo location isn't important result order.
                .Query(q => +q
                    .MultiMatch(m => m
                        .Query(searchText)
                        .Type(TextQueryType.BestFields)
                        .Fields(f => f.Field(p => p.Title).Field(p => p.Content))
                        .TieBreaker(0.3)
                        .MinimumShouldMatch("75%"))));

            return result.Documents.ToList();
        }
    }
}