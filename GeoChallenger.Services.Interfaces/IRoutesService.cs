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
        /// <param name="routeId">Route Id</param>
        /// <returns></returns>
        Task<RouteDto> GetRouteAsync(int userId, int routeId);

        /// <summary>
        ///     Create route
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="routeUpdateDto"></param>
        /// <returns></returns>
        Task<RouteDto> CreateRouteAsync(int userId, RouteUpdateDto routeUpdateDto);

        /// <summary>
        ///     Update route
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="routeId">Route Id</param>
        /// <param name="routeUpdateDto">Updated route information</param>
        /// <returns></returns>
        Task<RouteDto> UpdateRouteAsync(int userId, int routeId, RouteUpdateDto routeUpdateDto);

        /// <summary>
        ///     Delete route
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="routeId">Route id</param>
        /// <returns></returns>
        Task DeleteRouteAsync(int userId, int routeId);
    }
}
