using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Search.Documents;
using GeoChallenger.Services.Interfaces.DTO;


namespace GeoChallenger.Search.Providers
{
    public interface IPoisSearchProvider: IDocumentsSearchProvider<PoiDocument>
    {
        /// <summary>
        /// Returns all found poi.
        /// </summary>
        Task<IList<PoiDocument>> SearchAllAsync(string searchText = null, GeoBoundingBoxDto geoBoundingBox = null);
    }
}