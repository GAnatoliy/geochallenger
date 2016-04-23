namespace GeoChallenger.Web.Api.Models.Challenges
{
    public class ChallengeUpdateViewModel
    {
        /// <summary>
        /// Challenge task or question.
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// Correct answer to challenge.
        /// </summary>
        public string CorrectAnswer { get; set; }
    }
}