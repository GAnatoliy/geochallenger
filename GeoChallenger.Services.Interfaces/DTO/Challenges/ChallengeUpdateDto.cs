namespace GeoChallenger.Services.Interfaces.DTO.Challenges
{
    public class ChallengeUpdateDto
    {
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
    }
}