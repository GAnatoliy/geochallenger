namespace GeoChallenger.Services.Interfaces.DTO
{
    /// <summary>
    ///     Contains information about tag, e.g. location, data, etc.
    /// </summary>
    public class PoiDto
    {
        /// <summary>
        ///     POI Id
        /// </summary>
        public int PoiId { get; set; }

        /// <summary>
        ///     POI title
        /// </summary>
        public string Title { get; set; }
    }
}