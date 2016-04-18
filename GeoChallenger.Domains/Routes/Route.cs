using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
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
        ///     Route start point
        /// </summary>
        public DbGeography StartPoint { get; set; }

        /// <summary>
        ///     Route end point
        /// </summary>
        public DbGeography EndPoint { get; set; }

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
    }
}
