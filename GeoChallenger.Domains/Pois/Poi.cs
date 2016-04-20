using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using GeoChallenger.Domains.Routes;
using GeoChallenger.Domains.Users;


namespace GeoChallenger.Domains.Pois
{
    /// <summary>
    ///     Point of Interests
    /// </summary>
    public class Poi
    {
        #region Data

        /// <summary>
        /// POI Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// POI title
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
        /// POI location address
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

        #endregion

        #region Relations

        /// <summary>
        /// Poi routes. Relationship property
        /// </summary>
        public ICollection<Route> Routes { get; set; } = new HashSet<Route>();

        /// <summary>
        /// Poi owner id.
        /// </summary>
        public int UserId { get; protected set; }

        /// <summary>
        /// Poi owner user.
        /// </summary>
        public virtual User User { get; protected set; }

        #endregion

        public void AddUser(User user)
        {
            if (UserId != 0 || User != null) {
                throw new Exception($"Poi {Id} already has user.");
            }

            UserId = user.Id;
            User = user;
        }

        /// <summary>
        /// Mark current poi deleted
        /// </summary>
        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
