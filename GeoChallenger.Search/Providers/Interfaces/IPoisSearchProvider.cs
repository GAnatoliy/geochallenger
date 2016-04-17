using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Search.Documents;


namespace GeoChallenger.Search.Providers
{
    public interface IPoisSearchProvider: IDocumentsSearchProvider<PoiDocument>
    {
        /// <summary>
        /// Returns all found poi.
        /// </summary>
        Task<IList<PoiDocument>> SearchAllAsync(string searchText, int offset, int limit);
    }
}