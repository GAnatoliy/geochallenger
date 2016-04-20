using System.Collections.Generic;
using GeoChallenger.Services.Interfaces.DTO.Pois;

namespace GeoChallenger.Services.Interfaces.DTO.Routes
{
    /// <summary>
    ///     Carry route
    /// </summary>
    public class RouteDto
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
        public List<PoiDto> Pois { get; set; }
    }
}
