using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Database;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Search.Documents;
using GeoChallenger.Search.Providers.Interfaces;
using GeoChallenger.Services.Core;
using GeoChallenger.Services.Helpers;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO;
using GeoChallenger.Services.Interfaces.DTO.Pois;
using GeoChallenger.Services.Interfaces.Enums;
using GeoChallenger.Services.Interfaces.Exceptions;
using GeoChallenger.Services.Queries;
using Mehdime.Entity;
using NLog;


namespace GeoChallenger.Services
{
    public class PoisService: IPoisService
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;
        private readonly ISearchIndexer _searchIndexer;
        private readonly IPoisSearchProvider _poisSearchProvider;

        const int DEFAULT_POINTS_FOR_CHECKIN = 10;

        public PoisService(IMapper mapper, IDbContextScopeFactory dbContextScopeFactory, ISearchIndexer searchIndexer, IPoisSearchProvider poisSearchProvider)
        {
            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (dbContextScopeFactory == null) {
                throw new ArgumentNullException(nameof(dbContextScopeFactory));
            }
            if (searchIndexer == null) {
                throw new ArgumentNullException(nameof(searchIndexer));
            }
            if (poisSearchProvider == null) {
                throw new ArgumentNullException(nameof(poisSearchProvider));
            }
            _mapper = mapper;
            _dbContextScopeFactory = dbContextScopeFactory;
            _searchIndexer = searchIndexer;
            _poisSearchProvider = poisSearchProvider;
        }

        #region Queries
        public async Task<PoiDto> GetPoiAsync(int poiId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var poi = await context.Pois.FindAsync(poiId);
                if (poi == null || poi.IsDeleted) {
                    return null;
                }

                return _mapper.Map<PoiDto>(poi);
            }
        }

        public async Task<IList<SearchPoiResultDto>> SearchPoisAsync(string query = null, GeoBoundingBoxDto geoBoundingBox = null)
        {
            var pois = await _poisSearchProvider.SearchAllAsync(query, geoBoundingBox);
            return _mapper.Map<IList<SearchPoiResultDto>>(pois);
        }

        public async Task<IList<PoiDto>> SearchSimilarPoiAsync(int samplePoiId, int take)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var poi = await context.Pois.FindAsync(samplePoiId);
                if (poi == null || poi.IsDeleted) {
                    return new List<PoiDto>();
                }

                var similarPois = await _poisSearchProvider.SearchSimilarPoiAsync(
                    samplePoiId, poi.Location.Latitude.Value, poi.Location.Longitude.Value, take);

                return _mapper.Map<IList<PoiDto>>(similarPois);
            }
        }

        public async Task<IList<PoiDto>> GetUserPoisAsync(int ownerId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var pois = await context.Pois
                    .Where(p => !p.IsDeleted && p.OwnerId == ownerId)
                    .ToListAsync();

                return _mapper.Map<IList<PoiDto>>(pois);
            }
        }

        public async Task<IList<PoiMediaDto>> GetPoiMediaAsync(int poiId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var media = await dbContextScope.DbContexts.Get<GeoChallengerContext>().PoiMedia
                    .GetPoiMedia(poiId)
                    .ToListAsync();

                return _mapper.Map<IList<PoiMediaDto>>(media);
            }
        }

        #endregion

        #region Commands
        public async Task<PoiDto> CreatePoiAsync(int userId, PoiUpdateDto poiUpdateDto)
        {
            Poi poi;

            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var user = await context.Users.FindAsync(userId);
                if (user == null) {
                    throw new ObjectNotFoundException($"Can't create poi by user with id {userId}, user doesn't exist.");
                }

                poi = _mapper.Map<Poi>(poiUpdateDto);

                poi.AddOwner(user);
                poi.Content = HtmlHelper.SanitizeHtml(poiUpdateDto.Content);
                poi.ContentPreview = GetContentPreview(poiUpdateDto.Content);

                context.Pois.Add(poi);
                
                await dbContextScope.SaveChangesAsync();
            }

            // Create search index.
            try {
                await _poisSearchProvider.IndexAsync(_mapper.Map<PoiDocument>(poi));
            } catch (Exception ex) {
                _log.Warn(ex, $"Can't create search index for poi with id {poi.Id}");
            }

            return _mapper.Map<PoiDto>(poi);
        }

        public async Task<PoiDto> UpdatePoiAsync(int poiId, PoiUpdateDto poiUpdateDto)
        {
            Poi poi;

            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                poi = await context.Pois.FindAsync(poiId);
                if (poi == null || poi.IsDeleted) {
                    throw new ObjectNotFoundException($"Poi with id {poiId} is not found");
                }

                _mapper.Map(poiUpdateDto, poi);

                poi.Content = HtmlHelper.SanitizeHtml(poiUpdateDto.Content);
                poi.ContentPreview = GetContentPreview(poiUpdateDto.Content);

                await dbContextScope.SaveChangesAsync();
            }

            // Update search index.
            try {
                await _poisSearchProvider.IndexAsync(_mapper.Map<PoiDocument>(poi));
            } catch (Exception ex) {
                _log.Warn(ex, $"Can't update search index for poi with id {poi.Id}");
            }

            return _mapper.Map<PoiDto>(poi);
        }

        public async Task DeletePoiAsync(int userId, int poiId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var user = await context.Users.FindAsync(userId);
                if (user == null) {
                    throw new ObjectNotFoundException($"Can't delete poi {poiId} by user with id {userId}, user doesn't exist.");
                }

                var poi = await context.Pois.FindAsync(poiId);
                if (poi == null || poi.IsDeleted) {
                    throw new ObjectNotFoundException($"Poi with id {poiId} is not found");
                }

                if (poi.OwnerId != userId) {
                    throw new BusinessLogicException(ErrorCode.DeletePermissionRequired,
                        $"Can't delete poi with id '{poiId}', user '{userId}' should be owner.");
                }
                
                poi.Delete();

                await context.SaveChangesAsync();
            }

            // Delete search index.
            try {
                await _poisSearchProvider.DeleteAsync(poiId);
            } catch (Exception ex) {
                _log.Warn(ex, $"Can't delete search index for poi with id {poiId}");
            }
        }

        public async Task UpdatePoisSearchIndexAsync()
        {
            await _searchIndexer.UpdateSearchIndexAsync<Poi, PoiDocument>(
                poi => !poi.IsDeleted,
                poi => poi.Id,
                poi => {
                    var poiDocument = new PoiDocument();
                    _mapper.Map(poi, poiDocument);
                    return Task.FromResult(poiDocument);
                },
                _poisSearchProvider);
        }

        public async Task CheckinPoiAsync(int userId, int poiId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var user = context.Users.Find(userId);
                if (user == null) {
                    throw new ObjectNotFoundException($"Can't checkin poi {poiId} by user with id {userId}, user doesn't exist.");
                }

                var poi = context.Pois.Find(poiId);
                if (poi == null || poi.IsDeleted) {
                    throw new ObjectNotFoundException($"Poi with id {poiId} is not found");
                }

                // Check if poi already checkedin.
                // NOTE: if this selection will work without joins? 
                var isCheckedIn = await context.PoiCheckins
                    .AnyAsync(pc => pc.UserId == userId && pc.PoiId == poiId);

                if (isCheckedIn) {
                    _log.Warn($"Poi {poiId} is already checked in by user {userId}");
                    return;
                }

                // If no write checkin record.
                var checkin = new PoiCheckin(DEFAULT_POINTS_FOR_CHECKIN, user, poi);

                // Update user's pois.
                user.Points += DEFAULT_POINTS_FOR_CHECKIN;

                context.PoiCheckins.Add(checkin);

                // TODO: add unique constraint to poiId-userId, and use it for
                // to prevent writing duplicated request. 

                await context.SaveChangesAsync();
            }
        }

        #endregion

        /// <summary>
        /// Process string by left plain text (removing html) and strip it to the limited length.
        /// </summary>
        /// <returns></returns>
        // TODO: move to helper and test.
        private string GetContentPreview(string content)
        {
            const int DEFAULT_LENGTH = 256;

            if (string.IsNullOrEmpty(content)) {
                return string.Empty;
            }

            return StringHelper.Cut(HtmlHelper.ConvertToText(content), DEFAULT_LENGTH);
        }
    }
}
