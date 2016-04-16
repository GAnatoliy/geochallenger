using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Domains;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO;

namespace GeoChallenger.Services
{
    public class PoisService: IPoisService
    {
        static readonly List<Poi> PoisStubList = new List<Poi> {
            new Poi { PoiId = 1, Title = "Stub POI 1" },
            new Poi { PoiId = 2, Title = "Stub POI 2" },
            new Poi { PoiId = 3, Title = "Stub POI 3" }
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
            return _mapper.Map<IList<PoiDto>>(PoisStubList);
        }
    }
}
