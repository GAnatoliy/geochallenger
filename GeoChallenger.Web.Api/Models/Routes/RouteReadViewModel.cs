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
        ///     Route start point latitude 
        /// </summary>
        public double StartPointLatitude { get; set; }

        /// <summary>
        ///     Route start point longitude 
        /// </summary>
        public double StartPointLongitude { get; set; }

        /// <summary>
        ///     Route end point latitude
        /// </summary>
        public double EndPointLatitude { get; set; }

        /// <summary>
        ///     Route end point longitude
        /// </summary>
        public double EndPointLongitude { get; set; }

        /// <summary>
        ///     Route distance bewteen start and end points in meters
        /// </summary>
        public double DistanceInMeters { get; set; }

        /// <summary>
        ///     Route points path
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        ///     Route POIs
        /// </summary>
        public IList<PoiPreviewViewModel> Pois { get; set; }
    }
}