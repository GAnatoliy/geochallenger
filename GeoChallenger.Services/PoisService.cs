using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Database;
using GeoChallenger.Database.Extensions;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Services.Helpers;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Pois;
using Mehdime.Entity;


namespace GeoChallenger.Services
{
    public class PoisService: IPoisService
    {
        static readonly List<Poi> PoisStubList = new List<Poi> {
            new Poi { Id = 1, Title = "Stub POI 1", Address = "Dobrovolskogo St, 1, Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.534159, 32.275574) },
            new Poi { Id = 2, Title = "Stub POI 2", Address = "Shevchenka St, 1, Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.515507, 32.262109) },
            new Poi { Id = 3, Title = "Stub POI 3", Address = "Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.500530, 32.232154) }
        };

        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        public PoisService(IMapper mapper, IDbContextScopeFactory dbContextScopeFactory)
        {
            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (dbContextScopeFactory == null) {
                throw new ArgumentNullException(nameof(dbContextScopeFactory));
            }
            _mapper = mapper;
            _dbContextScopeFactory = dbContextScopeFactory;
        }

        public async Task<IList<PoiDto>> SearchPoisAsync(string query)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                // TODO: add full text search.
                var poisQuery = context.Pois
                    .Where(p => !p.IsDeleted);

                if (!string.IsNullOrEmpty(query)) {
                    poisQuery = poisQuery.Where(p => p.Content.ToLower().Contains(query));
                }
                    
                var pois = await poisQuery.ToListAsync();

                return _mapper.Map<IList<PoiDto>>(pois);
            }
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
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var poi = _mapper.Map<Poi>(poiUpdateDto);

                poi.Content = HtmlHelper.SanitizeHtml(poiUpdateDto.Content);
                poi.ContentPreview = GetContentPreview(poiUpdateDto.Content);

                context.Pois.Add(poi);

                // TODO: minify and make preview.
                
                await dbContextScope.SaveChangesAsync();
                return _mapper.Map<PoiDto>(poi);
            }
        }

        public async Task<PoiDto> UpdatePoiAsync(int poiId, PoiUpdateDto poiUpdateDto)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var poi = await context.Pois.FindAsync(poiId);
                if (poi == null || poi.IsDeleted) {
                    throw new ObjectNotFoundException($"Poi with id {poiId} is not found");
                }

                _mapper.Map(poiUpdateDto, poi);

                poi.Content = HtmlHelper.SanitizeHtml(poiUpdateDto.Content);
                poi.ContentPreview = GetContentPreview(poiUpdateDto.Content);

                await dbContextScope.SaveChangesAsync();

                return _mapper.Map<PoiDto>(poi);
            }
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
