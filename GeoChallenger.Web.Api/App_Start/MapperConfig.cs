using AutoMapper;
using GeoChallenger.Services.Interfaces.DTO;
using GeoChallenger.Web.Api.Models;


namespace GeoChallenger.Web.Api
{
    public class MapperConfig
    {
        public static MapperConfiguration CreateMapperConfiguration()
        {
            // TODO: consider to sue profile in order to configure mapping from different places,
            // ex. http://stackoverflow.com/questions/35187475/autofac-and-automapper-new-api-configurationstore-is-gone
            return new MapperConfiguration(ConfigureMappings);
        }

        private static void ConfigureMappings(IMapperConfiguration config)
        {
            // Configure servce mappings.
            Services.MapperConfig.ConfigureMappings(config);

            // Congigure current project mappings.
            MapFromContractsToViewModels(config);
            MapFromViewModelsToContracts(config);
        }

        private static void MapFromContractsToViewModels(IMapperConfiguration config)
        {
            config.CreateMap<TagDto, TagViewModel>();
        }

        private static void MapFromViewModelsToContracts(IMapperConfiguration config)
        {
            
        }
    }
}