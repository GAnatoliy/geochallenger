﻿using System.Linq;
using GeoChallenger.Domains.Routes;

namespace GeoChallenger.Services.Queries
{
    /// <summary>
    ///     Routes queries
    /// </summary>
    public static class RoutesQueries
    {
        /// <summary>
        ///     Get routes
        /// </summary>
        /// <param name="query">Query</param>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        public static IQueryable<Route> GetRoutes(this IQueryable<Route> query, int userId)
        {
            return query.Where(r => !r.IsDeleted && r.UserId == userId);
        }

        /// <summary>
        ///     Get route
        /// </summary>
        /// <param name="query">Query</param>
        /// <param name="userId">User Id</param>
        /// <param name="routeId">Route Id</param>
        /// <returns></returns>
        public static IQueryable<Route> GetRoute(this IQueryable<Route> query, int userId, int routeId)
        {
            return query.Where(r => !r.IsDeleted && r.Id == routeId && r.UserId == userId);
        }
    }
}
