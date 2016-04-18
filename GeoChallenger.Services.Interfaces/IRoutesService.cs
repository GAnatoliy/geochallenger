using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO.Routes;

namespace GeoChallenger.Services.Interfaces
{
    public interface IRoutesService
    {
        /// <summary>
        ///     Get routes by user id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        Task<List<RouteDto>> GetRoutesAsync(int userId);
    }
}
