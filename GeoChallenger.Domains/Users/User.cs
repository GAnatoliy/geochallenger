using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Routes;

namespace GeoChallenger.Domains.Users
{
    /// <summary>
    ///     User
    /// </summary>
    public class User
    {
        #region Data

        /// <summary>
        /// User Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User Full Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User created date in UTC
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Points earned by user.
        /// </summary>
        public int Points { get; set; }
        #endregion

        #region Relations 

        /// <summary>
        /// User's accounts. 
        /// </summary>
        public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();

        /// <summary>
        /// User's routes.
        /// </summary>
        public virtual ICollection<Route> Routes { get; set; } = new HashSet<Route>();

        /// <summary>
        /// User's pois.
        /// </summary>
        public virtual ICollection<Poi> Pois { get; protected set; } = new Collection<Poi>();

        public virtual ICollection<PoiCheckin> Checkins { get; protected set; } = new Collection<PoiCheckin>(); 
        #endregion
    }
}
