namespace GeoChallenger.Services.Interfaces.DTO
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
