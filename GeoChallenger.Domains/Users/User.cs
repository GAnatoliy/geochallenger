using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;

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
        ///     User Firts Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     User Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     User created date in UTC
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     User Accounts. Relationship property
        /// </summary>
        public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    }
}
