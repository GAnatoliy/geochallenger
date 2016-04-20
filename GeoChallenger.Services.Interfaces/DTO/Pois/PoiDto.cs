using System;


namespace GeoChallenger.Services.Interfaces.DTO.Pois
{
    /// <summary>
    ///     Contains information about tag, e.g. location, data, etc.
    /// </summary>
    public class  PoiDto
    {
        /// <summary>
        ///     POI Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     POI title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Short preview of the content.
        /// </summary>
        public string ContentPreview { get; set; }

        /// <summary>
        /// Text content of the poi.
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
        /// POI creation date at UTC
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>
        /// Poi owner id.
        /// </summary>
        public int OwnerId { get; set; }
    }
}