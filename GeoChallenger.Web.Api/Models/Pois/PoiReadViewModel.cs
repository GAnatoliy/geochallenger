﻿namespace GeoChallenger.Web.Api.Models.Pois
{
    /// <summary>
    ///     POI read view model
    /// </summary>
    public class PoiReadViewModel
    {
        /// <summary>
        ///     POI Id
        /// </summary>
        public int PoiId { get; set; }

        /// <summary>
        ///     POI title
        /// </summary>
        public string Title { get; set; }

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
    }
}