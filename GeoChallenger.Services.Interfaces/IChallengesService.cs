using System.Collections.Generic;
using System.Threading.Tasks;
using GeoChallenger.Services.Interfaces.DTO.Challenges;


namespace GeoChallenger.Services.Interfaces
{
    public interface IChallengesService
    {
        #region Queries

        /// <summary>
        /// Get challenge by id.
        /// </summary>
        Task<ChallengeDto> GetChallengeAsync(int challengeId);

        /// <summary>
        /// Returns challenges related to this poi.
        /// </summary>
        Task<IList<ChallengeDto>> GetChallengesForPoiAsync(int poiId);

        /// <summary>
        /// Returns challenges created by specified user.
        /// </summary>
        Task<IList<ChallengeDto>> GetChallengesCreatedByUserAsync(int userId);

        #endregion

        #region Commands

        /// <summary>
        /// Analyzes challenge's answer and returns true if it is correct.
        /// Adds points reward for user.
        /// </summary>
        Task<bool> AnswerToChallengeAsync(int userId, int challengeId, string answer);

        /// <summary>
        /// Create new challenge for poi.
        /// </summary>
        Task<ChallengeDto> CreateChallengeAsync(int userId, int poiId, ChallengeUpdateDto challengeUpdateDto);

        /// <summary>
        /// Update challenge for poi.
        /// </summary>
        Task<ChallengeDto> UpdateChallengeAsync(int userId, int challengeId, ChallengeUpdateDto challengeUpdateDto);

        /// <summary>
        /// Remove challenge from the system.
        /// </summary>
        Task DeleteChallengeAsync(int userId, int challengeId);

        #endregion
    }
}