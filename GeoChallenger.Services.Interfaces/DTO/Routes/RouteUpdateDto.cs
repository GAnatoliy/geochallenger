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
