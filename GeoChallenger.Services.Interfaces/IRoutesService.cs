using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO.Routes;

namespace GeoChallenger.Services.Interfaces
{
    public interface IRoutesService
    {
        /// <summary>
        ///     Get user routes
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        Task<List<RouteDto>> GetRoutesAsync(int userId);

        /// <summary>
        ///     Get user route
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="routeId">Rotue Id</param>
        /// <returns></returns>
        Task<RouteDto> GetRouteAsync(int userId, int routeId);
    }
}
