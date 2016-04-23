using GeoChallenger.Services.Interfaces.DTO.Media;

namespace GeoChallenger.Services.Interfaces.DTO.Pois
{
    /// <summary>
    ///     Carry new poi media
    /// </summary>
    public class PoiMediaUpdateDto
    {
        /// <summary>
        ///     Media filename with extension
        /// </summary>
        public string MediaName { get; set; }

        /// <summary>
        ///     Media type
        /// </summary>
        public MediaTypeDto MediaType { get; set; }

        /// <summary>
        ///     Poi media content type
        /// </summary>
        public string ContentType { get; set; }
    }
}
