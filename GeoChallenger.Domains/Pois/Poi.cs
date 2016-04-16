using System.Data.Entity.Spatial;

namespace GeoChallenger.Domains.Pois
{
    /// <summary>
    ///     Point of Interests
    /// </summary>
    public class Poi
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
        public DbGeography Location { get; set; }
    }
}
