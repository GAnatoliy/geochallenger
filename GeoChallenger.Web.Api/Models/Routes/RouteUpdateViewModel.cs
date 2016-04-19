using System.Collections.Generic;

namespace GeoChallenger.Web.Api.Models.Routes
{
    /// <summary>
    ///     View model for update route
    /// </summary>
    public class RouteUpdateViewModel
    {
        /// <summary>
        ///     Route name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Route start point coordinates 
        /// </summary>
        public string StartPoint { get; set; }

        /// <summary>
        ///     Route end point coordinates
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        ///     New route POIs ids
        /// </summary>
        public IList<int> PoisIds { get; set; }
    }
}