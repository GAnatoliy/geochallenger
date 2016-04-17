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
using GeoChallenger.Search.Providers;
using GeoChallenger.Services.Core;
using GeoChallenger.Services.Helpers;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Pois;
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

        public async Task<IList<SearchPoiResultDto>> SearchPoisAsync(string query)
        {
            var pois = await _poisSearchProvider.SearchAllAsync(query);
            return _mapper.Map<IList<SearchPoiResultDto>>(pois);
        }

        public async Task<PoiDto> GetPoiAsync(int poiId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var poi = await context.Pois
                    .Where(p => !p.IsDeleted && p.Id == poiId)
                    .SingleOrDefaultAsync();

                return _mapper.Map<PoiDto>(poi);
            }
        }

        public async Task<PoiDto> CreatePoiAsync(PoiUpdateDto poiUpdateDto)
        {
            Poi poi;

            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                poi = _mapper.Map<Poi>(poiUpdateDto);

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

        public async Task DeletePoiAsync(int poiId)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var poi = await context.Pois.FindAsync(poiId);
                if (poi == null || poi.IsDeleted) {
                    throw new ObjectNotFoundException($"Poi with id {poiId} is not found");
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
