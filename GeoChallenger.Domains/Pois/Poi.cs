using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Spatial;
using GeoChallenger.Domains.Challenges;
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

        /// <summary>
        /// POI creation date at UTC
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        #endregion

        #region Relations

        /// <summary>
        /// Poi routes. Relationship property
        /// </summary>
        public virtual ICollection<Route> Routes { get; set; } = new HashSet<Route>();

        /// <summary>
        /// Poi owner id.
        /// </summary>
        public int OwnerId { get; protected set; }

        /// <summary>
        /// Poi owner user.
        /// </summary>
        public virtual User Owner { get; protected set; }

        /// <summary>
        /// Checkins of this poi.
        /// </summary>
        public virtual ICollection<PoiCheckin> Checkins { get; protected set; } = new Collection<PoiCheckin>();

        /// <summary>
        /// Media of this poi
        /// </summary>
        public virtual ICollection<PoiMedia> Media { get; set; } = new HashSet<PoiMedia>();

        /// <summary>
        /// Challenges created for this poi.
        /// </summary>
        public virtual ICollection<Challenge> Challengeses { get; protected set; } = new Collection<Challenge>();

        #endregion

        public void AddOwner(User user)
        {
            if (OwnerId != 0 || Owner != null) {
                throw new Exception($"Poi {Id} already has user.");
            }

            OwnerId = user.Id;
            Owner = user;
        }

        /// <summary>
        /// Add media to the poi 
        /// </summary>
        /// <param name="poiMedia"></param>
        public void AddMedia(PoiMedia poiMedia)
        {
            Media.Add(poiMedia);
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
