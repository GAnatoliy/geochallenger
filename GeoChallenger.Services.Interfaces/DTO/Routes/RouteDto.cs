﻿using System.Collections.Generic;
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
        public List<PoiDto> Pois { get; set; }
    }
}
