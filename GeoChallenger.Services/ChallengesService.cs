using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Database;
using GeoChallenger.Domains.Challenges;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Challenges;
using GeoChallenger.Services.Interfaces.Enums;
using GeoChallenger.Services.Interfaces.Exceptions;
using Mehdime.Entity;
using NLog;


namespace GeoChallenger.Services
{
    public class ChallengesService: IChallengesService
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        public ChallengesService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            if (dbContextScopeFactory == null) {
                throw new ArgumentNullException(nameof(dbContextScopeFactory));
            }
            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        #region Queries

        public async Task<ChallengeDto> GetChallengeAsync(int challengeId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var challenge = await context.Challenges.FindAsync(challengeId);
                if (challenge == null || challenge.IsDeleted) {
                    return null;
                }

                return _mapper.Map<ChallengeDto>(challenge);
            }
        }

        public async Task<IList<ChallengeDto>> GetChallengesForPoiAsync(int poiId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var challenges = await context.Challenges
                    .Where(c => !c.IsDeleted && c.PoiId == poiId)
                    .ToListAsync();

                return _mapper.Map<IList<ChallengeDto>>(challenges);
            }
        }

        public async Task<IList<ChallengeDto>> GetChallengesCreatedByUserAsync(int userId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var challenges = await context.Challenges
                    .Where(c => !c.IsDeleted && c.CreatorId == userId)
                    .ToListAsync();

                return _mapper.Map<IList<ChallengeDto>>(challenges);
            }
        }

        #endregion

        #region Commands

        public async Task<bool> AnswerToChallengeAsync(int userId, int challengeId, string answer)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var user = await context.Users.FindAsync(userId);
                if (user == null) {
                    throw new ObjectNotFoundException($"Can't answer to challenge {challengeId} by user with id {userId}, user doesn't exist.");
                }

                var challenge = await context.Challenges.FindAsync(challengeId);
                if (challenge == null || challenge.IsDeleted) {
                    throw new ObjectNotFoundException($"Can't answer to challenge {challengeId} by user with id {userId}, challenge doesn't exist.");
                }

                // User can't answer to own challenges.
                if (challenge.CreatorId == userId) {
                    throw new BusinessLogicException(ErrorCode.AnswerToOwnChallenge,
                        $"User {userId} created challenge {challengeId}, so it can't answer to it.");
                }

                // User can't answer on the same challenge if it answered correct.
                var isAlreadyAnsweredCorrect = context.ChallengeAnswers
                    .Any(c => c.ChallengeId == challengeId && c.UserId == userId && c.IsCorrect);
                if (isAlreadyAnsweredCorrect) {
                    _log.Warn($"User {userId} already answered correct for challenge {challengeId}.");
                    return challenge.IsCorrectAnswer(answer);
                }

                if (!challenge.IsCorrectAnswer(answer)) {
                    _log.Info($"User {userId} answered to challenge {challengeId} - '{Challenge.SanitizeAnswer(answer)}', it isn't correct answer.");
                } else {
                    _log.Info($"User {userId} answered to challenge {challengeId} - '{Challenge.SanitizeAnswer(answer)}', it is correct answer.");
                    user.Points += challenge.PointsReward;
                }

                // TODO: make sure that user has only one correct answer (by constraints).
                challenge.Answer(user, answer);
                
                await dbContextScope.SaveChangesAsync();

                return challenge.IsCorrectAnswer(answer);
            }
        }

        public async Task<ChallengeDto> CreateChallengeAsync(int userId, int poiId, ChallengeUpdateDto challengeUpdateDto)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var user = await context.Users.FindAsync(userId);
                if (user == null) {
                    throw new ObjectNotFoundException($"Can't create challenge by user with id {userId} for poi {poiId}, user doesn't exist.");
                }

                var poi = await context.Pois.FindAsync(poiId);
                if (poi == null || poi.IsDeleted) {
                    throw new ObjectNotFoundException($"Can't create challenge by user with id {userId} for poi {poiId}, challenge doesn't exist.");
                }

                var challenge = _mapper.Map<Challenge>(challengeUpdateDto);
                challenge.Creator = user;
                challenge.Poi = poi;

                context.Challenges.Add(challenge);

                await dbContextScope.SaveChangesAsync();

                return _mapper.Map<ChallengeDto>(challenge);
            }
        }

        public async Task<ChallengeDto> UpdateChallengeAsync(int userId, int challengeId, ChallengeUpdateDto challengeUpdateDto)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var challenge = await context.Challenges.FindAsync(challengeId);
                if (challenge == null || challenge.IsDeleted) {
                    throw new ObjectNotFoundException($"Can't update challenge {challengeId} by user with id {userId}, challenge doesn't exist.");
                }

                var user = await context.Users.FindAsync(userId);
                if (user == null) {
                    throw new ObjectNotFoundException($"Can't update challenge {challengeId} by user with id {userId}, user doesn't exist.");
                }

                if (challenge.CreatorId != userId) {
                    throw new BusinessLogicException(ErrorCode.UpdatePermissionRequired,
                        $"Can't update challenge with id '{challengeId}', user '{userId}' should be creator.");
                }

                _mapper.Map(challengeUpdateDto, challenge);
                challenge.UpdatedAtUtc = DateTime.UtcNow;

                await dbContextScope.SaveChangesAsync();

                return _mapper.Map<ChallengeDto>(challenge);
            }
        }

        public async Task DeleteChallengeAsync(int userId, int challengeId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var challenge = await context.Challenges.FindAsync(challengeId);
                if (challenge == null || challenge.IsDeleted) {
                    throw new ObjectNotFoundException($"Can't delete challenge {challengeId} by user with id {userId}, challenge doesn't exist.");
                }

                var user = await context.Users.FindAsync(userId);
                if (user == null) {
                    throw new ObjectNotFoundException($"Can't update challenge {challengeId} by user with id {userId}, user doesn't exist.");
                }

                if (challenge.CreatorId != userId) {
                    throw new BusinessLogicException(ErrorCode.DeletePermissionRequired,
                        $"Can't delete challenge with id '{challengeId}', user '{userId}' should be creator.");
                }

                challenge.Delete();

                await dbContextScope.SaveChangesAsync();
            }
        }

        #endregion
    }
}