using GeoChallenger.Services.Interfaces.DTO.Media;

namespace GeoChallenger.Services.Interfaces.DTO.Pois
{
    /// <summary>
    ///     Carry poi media
    /// </summary>
    public class PoiMediaDto
    {
        /// <summary>
        ///     Poi media Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Poi media filename with extension
        /// </summary>
        public string MediaName { get; set; }

        /// <summary>
        ///     Poi media type
        /// </summary>
        public MediaTypeDto MediaType { get; set; }

        /// <summary>
        ///     Media creator user Id
        /// </summary>
        public int UserId { get; set; }
    }
}
