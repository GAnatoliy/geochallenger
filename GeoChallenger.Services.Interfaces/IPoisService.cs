using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO;
using GeoChallenger.Services.Interfaces.DTO.Pois;

namespace GeoChallenger.Services.Interfaces
{
    /// <summary>
    ///     Provide access to the tags.
    /// </summary>
    public interface IPoisService
    {
        #region Queries
        /// <summary>
        ///     Returns all pois in system
        /// </summary>
        /// <returns></returns>
        Task<IList<SearchPoiResultDto>> SearchPoisAsync(string query = null, GeoBoundingBoxDto geoBoundingBox = null);

        /// <summary>
        ///     Get poi by Id
        /// </summary>
        /// <param name="poiId">Poi Id</param>
        /// <returns>Return poi</returns>
        Task<PoiDto> GetPoiAsync(int poiId);

        /// <summary>
        /// Returns similar poi to the sample (priority is given for near pois).
        /// </summary>
        Task<IList<PoiDto>> SearchSimilarPoiAsync(int samplePoiId, int take);

        /// <summary>
        /// Returns pois created by current user.
        /// </summary>
        Task<IList<PoiDto>> GetUserPoisAsync(int ownerId);
        #endregion

        #region Commands 
        /// <summary>
        /// Create new poi.
        /// </summary>
        /// <param name="poiUpdateDto">Data of new poi.</param>
        /// <returns>Returns new created poi.</returns>
        Task<PoiDto> CreatePoiAsync(int userId, PoiUpdateDto poiUpdateDto);

        /// <summary>
        ///     Update poi by Id
        /// </summary>
        /// <param name="poiId">Poi id</param>
        /// <param name="poiUpdateDto">Updated content for poi</param>
        /// <returns></returns>
        Task<PoiDto> UpdatePoiAsync(int poiId, PoiUpdateDto poiUpdateDto);

        /// <summary>
        ///     Delete poi by Id
        /// </summary>
        /// <param name="poiId">Poi Id</param>
        /// <returns></returns>
        Task DeletePoiAsync(int userId, int poiId);

        /// <summary>
        /// Update search index of all poi.
        /// </summary>
        /// <returns></returns>
        Task UpdatePoisSearchIndexAsync();
        #endregion
    }
}
