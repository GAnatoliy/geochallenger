using System;

namespace GeoChallenger.Domains.Users
{
    /// <summary>
    ///     Account
    /// </summary>
    public class Account
    {
        /// <summary>
        ///     Social account Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Social network account unique identificator
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        ///     Social account type
        /// </summary>
        public AccountType Type { get; set; }

        /// <summary>
        ///     Social account attached date at UTC
        /// </summary>
        public DateTime AttachedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        ///     Linked with this account user Id. Relationship property
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     One-to-many (user - accounts) relationship. Relationship property
        /// </summary>
        public virtual User User { get; set; }
    }
}
