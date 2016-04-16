using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO;

namespace GeoChallenger.Services.Interfaces
{
    /// <summary>
    ///     Provide access to the tags.
    /// </summary>
    public interface IPoisService
    {
        /// <summary>
        ///     Returns all pois in system
        /// </summary>
        /// <returns></returns>
        Task<IList<PoiDto>> GetPoisAsync();

        /// <summary>
        ///     Get poi by Id
        /// </summary>
        /// <param name="poiId">Poi Id</param>
        /// <returns>Return poi</returns>
        Task<PoiDto> GetPoiAsync(int poiId);

        /// <summary>
        ///     Update poi by Id
        /// </summary>
        /// <param name="poiId">Poi id</param>
        /// <param name="poiUpdateDto">Updated content for poi</param>
        /// <returns></returns>
        Task UpdatePoiAsync(int poiId, PoiUpdateDto poiUpdateDto);

        /// <summary>
        ///     Delete poi by Id
        /// </summary>
        /// <param name="poiId">Poi Id</param>
        /// <returns></returns>
        Task DeletePoiAsync(int poiId);
    }
}
