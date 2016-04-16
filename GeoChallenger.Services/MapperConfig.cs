using AutoMapper;
using GeoChallenger.Domains;
using GeoChallenger.Services.Interfaces.DTO;

namespace GeoChallenger.Services
{
    public class MapperConfig
    {
        public static void ConfigureMappings(IMapperConfiguration config)
        {
            MapFromDomainsToContracts(config);
            MapFromContractsToDomains(config);
        }

        private static void MapFromDomainsToContracts(IMapperConfiguration config)
        {
            config.CreateMap<Poi, PoiDto>();
        }

        private static void MapFromContractsToDomains(IMapperConfiguration config)
        {
            config.CreateMap<PoiUpdateDto, Poi>();
        }
    }
}