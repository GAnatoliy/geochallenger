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

        /// <summary>
        /// Returns similar poi to the sample (priority is given for near pois).
        /// </summary>
        Task<IList<PoiDocument>> SearchSimilarPoiAsync(int samplePoiId, double samplePoiLatitude, double samplePoiLongitude, int take);
    }
}