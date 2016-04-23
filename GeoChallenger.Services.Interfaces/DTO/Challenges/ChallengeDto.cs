using System;


namespace GeoChallenger.Services.Interfaces.DTO.Challenges
{
    public class ChallengeDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Challenge task or question.
        /// </summary>
        public string Task { get; set; }

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

        /// <summary>
        /// Poi where challenge is attached.
        /// </summary>
        public int PoiId { get; set; }

        /// <summary>
        /// Id of challenge creator.
        /// </summary>
        public int CreatorId { get; set; }
    }
}