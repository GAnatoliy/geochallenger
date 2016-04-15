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
            config.CreateMap<Tag, TagDto>();
        }

        private static void MapFromContractsToDomains(IMapperConfiguration config)
        {
            
        }
    }
}