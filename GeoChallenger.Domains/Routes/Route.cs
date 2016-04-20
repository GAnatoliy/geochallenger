using System;
using System.Collections.Generic;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Users;

namespace GeoChallenger.Domains.Routes
{
    /// <summary>
    ///     Route
    /// </summary>
    public class Route
    {
        /// <summary>
        ///     Route Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Route name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Route start point latitude 
        /// </summary>
        public double StartPointLatitude { get; set; }

        /// <summary>
        ///     Route start point longitude 
        /// </summary>
        public double StartPointLongitude { get; set; }

        /// <summary>
        ///     Route end point latitude
        /// </summary>
        public double EndPointLatitude { get; set; }

        /// <summary>
        ///     Route end point longitude
        /// </summary>
        public double EndPointLongitude { get; set; }

        /// <summary>
        ///     Route distance bewteen start and end points in meters
        /// </summary>
        public double DistanceInMeters { get; set; }

        /// <summary>
        ///     Route points path
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        ///     Route creation date at UTC
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     Is route deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     Route creator User Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     Related user. Relationship property
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        ///     Route POIs. Relationship property
        /// </summary>
        public virtual ICollection<Poi> Pois { get; set; } = new HashSet<Poi>();

        /// <summary>
        ///     Mark current route deleted
        /// </summary>
        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
