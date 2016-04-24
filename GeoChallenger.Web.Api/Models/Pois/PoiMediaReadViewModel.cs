namespace GeoChallenger.Web.Api.Models.Pois
{
    /// <summary>
    ///     View model for poi media
    /// </summary>
    public class PoiMediaReadViewModel
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
        ///     Poi media absolute url
        /// </summary>
        public string MediaUrl { get; set; }

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