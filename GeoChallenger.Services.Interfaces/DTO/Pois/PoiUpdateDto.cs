﻿using System.Collections.Generic;

namespace GeoChallenger.Services.Interfaces.DTO.Pois
{
    /// <summary>
    ///     Carry updated poi information
    /// </summary>
    public class PoiUpdateDto
    {
        /// <summary>
        ///     Poi title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Text content of poi.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     POI location address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     POI location latitude
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        ///     POI location Longitude
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        ///     POI media
        /// </summary>
        public IList<PoiMediaUpdateDto> Media { get; set; }
    }
}
