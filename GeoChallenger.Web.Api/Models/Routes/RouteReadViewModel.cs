using System.Collections.Generic;
using GeoChallenger.Web.Api.Models.Pois;

namespace GeoChallenger.Web.Api.Models.Routes
{
    /// <summary>
    ///     View model for route
    /// </summary>
    public class RouteReadViewModel
    {
        /// <summary>
        ///     Route Id
        /// </summary>
        public int Id { get; set; }

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
        ///     Route POIs
        /// </summary>
        public List<PoiReadViewModel> Pois { get; set; }
    }
}