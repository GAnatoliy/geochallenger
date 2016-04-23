using System;
using System.Collections.Generic;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Users;


namespace GeoChallenger.Domains.Challenges
{
    /// <summary>
    /// Challenge at the POI.
    /// </summary>
    public class Challenge
    {
        #region Date

        public int Id { get; set; }

        /// <summary>
        /// Challenge task or question.
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// Correct answer to challenge.
        /// </summary>
        public string CorrectAnswer { get; set; }

        /// <summary>
        /// Number of points get by user for solving challange.
        /// </summary>
        public int PointsReward { get; set; }

        /// <summary>
        /// Challenge creation date at UTC.
        /// </summary>
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when challenge was last time updated.
        /// </summary>
        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        #endregion

        #region Relations

        /// <summary>
        /// Answers given by users.
        /// </summary>
        public ICollection<ChallengeAnswer> Answers { get; set; }

        /// <summary>
        /// Poi where challenge is attached.
        /// </summary>
        public int PoiId { get; set; }

        /// <summary>
        /// Poi where challenge is attached.
        /// </summary>
        public virtual Poi Poi { get; set; }

        /// <summary>
        /// Id of challenge creator.
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// User that created challenge.
        /// </summary>
        public virtual User Creator { get; set; }
        #endregion
    }
}