using System;
using GeoChallenger.Domains.Media;

namespace GeoChallenger.Domains.Pois
{
    /// <summary>
    ///     Poi media
    /// </summary>
    public class PoiMedia
    {
        /// <summary>
        ///     Poi media Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Media filename with extension
        /// </summary>
        public string MediaName { get; set; }

        /// <summary>
        ///     Media type
        /// </summary>
        public MediaType MediaType { get; set; }

        /// <summary>
        ///     Media creation date
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>
        ///     Media creator user Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     Related poi Id
        /// </summary>
        public int PoiId { get; set; }

        /// <summary>
        ///     Related poi
        /// </summary>
        public virtual Poi Poi { get; set; }
    }
}
