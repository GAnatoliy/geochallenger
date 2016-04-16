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
        ///     Returns all POIs in system
        /// </summary>
        /// <returns></returns>
        Task<IList<PoiDto>> GetPoisAsync();
    }
}
