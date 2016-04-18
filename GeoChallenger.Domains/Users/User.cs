using System;
using System.Collections.Generic;
using GeoChallenger.Domains.Routes;

namespace GeoChallenger.Domains.Users
{
    /// <summary>
    ///     User
    /// </summary>
    public class User
    {
        /// <summary>
        ///     User Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     User Full Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     User created date in UTC
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     User Accounts. Relationship property
        /// </summary>
        public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();

        /// <summary>
        ///     User Routes. Relationship property
        /// </summary>
        public virtual ICollection<Route> Routes { get; set; } = new HashSet<Route>();
    }
}
