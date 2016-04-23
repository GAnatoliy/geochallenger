namespace GeoChallenger.Web.Api.Models.Pois
{
    /// <summary>
    ///     View Model for create poi media
    /// </summary>
    public class PoiMediaUpdateViewModel
    {
        /// <summary>
        ///     Poi media filename
        /// </summary>
        public string MediaName { get; set; }

        /// <summary>
        ///     Poi media content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///     Poi media type
        /// </summary>
        public MediaTypeViewModel MediaType { get; set; }
    }
}