using System.Collections.Generic;
using System.Data.Entity.Spatial;
using GeoChallenger.Domains.Routes;

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
        public DbGeography Location { get; set; }

        /// <summary>
        /// Indicates that POI is deleted and shouldn't shown for user.
        /// </summary>
        public bool IsDeleted { get; protected set; }

        /// <summary>
        ///     Poi routes. Relationship property
        /// </summary>
        public ICollection<Route> Routes { get; set; } = new HashSet<Route>();

        /// <summary>
        ///     Mark current poi deleted
        /// </summary>
        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
