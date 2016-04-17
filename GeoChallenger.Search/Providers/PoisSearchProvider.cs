using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Search.Documents;


namespace GeoChallenger.Search.Providers
{
    public class PoisSearchProvider: DocumentsSearchProvider<PoiDocument>, IPoisSearchProvider
    {
        public PoisSearchProvider(SearchSettings searchSettings) : base(searchSettings)
        {
        }

        public Task<IList<PoiDocument>> SearchAllAsync(string searchText, int offset, int limit)
        {
            throw new System.NotImplementedException();
        }
    }
}