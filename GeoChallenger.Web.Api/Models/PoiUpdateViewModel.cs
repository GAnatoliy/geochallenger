namespace GeoChallenger.Web.Api.Models
{
    /// <summary>
    ///     View model for update poi by user
    /// </summary>
    public class PoiUpdateViewModel
    {
        /// <summary>
        ///     Poi Id
        /// </summary>
        public int PoiId { get; set; }

        /// <summary>
        ///     Poi title
        /// </summary>
        public string Title { get; set; }
    }
}