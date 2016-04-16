using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Database;
using GeoChallenger.Database.Extensions;
using GeoChallenger.Domains;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO;

namespace GeoChallenger.Services
{
    public class PoisService: IPoisService
    {
        static readonly List<Poi> PoisStubList = new List<Poi> {
            new Poi { PoiId = 1, Title = "Stub POI 1", Address = "Dobrovolskogo St, 1, Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.534159, 32.275574) },
            new Poi { PoiId = 2, Title = "Stub POI 2", Address = "Shevchenka St, 1, Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.515507, 32.262109) },
            new Poi { PoiId = 3, Title = "Stub POI 3", Address = "Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.500530, 32.232154) }
        };

        private readonly IMapper _mapper;

        public PoisService(IMapper mapper)
        {
            if (mapper == null) {
                throw new ArgumentNullException(nameof(mapper));
            }
            _mapper = mapper;
        }

        public async Task<IList<PoiDto>> GetPoisAsync()
        {
            using (var context = new GeoChallengerContext()) {
                var pois = context.Pois.ToList();

                return _mapper.Map<IList<PoiDto>>(pois);
            }
        }

        public async Task<PoiDto> GetPoiAsync(int poiId)
        {
            return _mapper.Map<PoiDto>(PoisStubList.SingleOrDefault(p => p.PoiId == poiId));
        }

        public async Task UpdatePoiAsync(int poiId, PoiUpdateDto poiUpdateDto)
        {
            var poi = PoisStubList.SingleOrDefault(p => p.PoiId == poiId);
            if (poi == null) {
                throw new ObjectNotFoundException($"Poi with id {poiId} is not found");
            }

            _mapper.Map(poiUpdateDto, poi);
        }

        public async Task DeletePoiAsync(int poiId)
        {
            var poi = PoisStubList.SingleOrDefault(p => p.PoiId == poiId);
            if (poi == null) {
                throw new ObjectNotFoundException($"Poi with id {poiId} is not found");
            }

            PoisStubList.Remove(poi);
        }
    }
}
