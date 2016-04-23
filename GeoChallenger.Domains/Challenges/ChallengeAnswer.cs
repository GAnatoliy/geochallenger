using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Users;


namespace GeoChallenger.Domains.Challenges
{
    /// <summary>
    /// Answers to the challenge.
    /// </summary>
    public class ChallengeAnswer
    {
        #region Data
        
        public int Id { get; set; }

        /// <summary>
        /// User's answer to challenge.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Indicates if answer is correct.
        /// </summary>
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Points earned in case of correct answer.
        /// </summary>
        public int EarnedPoints { get; set; }

        #endregion

        #region Relations

        /// <summary>
        /// Answer was given for challenge with this id.
        /// </summary>
        public int ChallengeId { get; set; }

        /// <summary>
        /// Answer was given for this challenge.
        /// </summary>
        public virtual Challenge Challenge { get; set; }

        /// <summary>
        /// If of user that answered for challenge.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User that answered for challenge.
        /// </summary>
        public virtual User User { get; set; }

        #endregion
    }
}