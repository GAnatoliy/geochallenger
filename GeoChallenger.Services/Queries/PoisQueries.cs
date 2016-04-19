using System.Collections.Generic;
using System.Linq;
using GeoChallenger.Domains.Pois;

namespace GeoChallenger.Services.Queries
{
    /// <summary>
    ///     POIs queries
    /// </summary>
    public static class PoisQueries
    {
        /// <summary>
        ///     Get POIs
        /// </summary>
        /// <param name="query">IQueryable</param>
        /// <param name="routeId">Route Id</param>
        /// <returns></returns>
        public static IQueryable<Poi> GetPois(this IQueryable<Poi> query, int routeId)
        {
            return query.Where(p => !p.IsDeleted && p.Routes.Any(r => r.Id == routeId));
        }

        /// <summary>
        ///     Get POIs
        /// </summary>
        /// <param name="query">IQueryable</param>
        /// <param name="routesIds">Routes Ids</param>
        /// <returns></returns>
        public static IQueryable<Poi> GetPois(this IQueryable<Poi> query, List<int> routesIds)
        {
            return query.Where(p => !p.IsDeleted && p.Routes.Any(r => routesIds.Contains(r.Id)));
        }
    }
}
