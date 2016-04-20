using System.Collections.Generic;

namespace GeoChallenger.Services.Interfaces.DTO.Routes
{
    /// <summary>
    ///     Carry changes for update route
    /// </summary>
    public class RouteUpdateDto
    {
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
        ///     New route POIs ids
        /// </summary>
        public IList<int> PoisIds { get; set; }
    }
}
