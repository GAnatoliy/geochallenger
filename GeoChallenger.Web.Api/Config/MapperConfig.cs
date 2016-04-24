using AutoMapper;
using GeoChallenger.Services.Interfaces.DTO.Challenges;
using GeoChallenger.Services.Interfaces.DTO.Media;
using GeoChallenger.Services.Interfaces.DTO.Pois;
using GeoChallenger.Services.Interfaces.DTO.Routes;
using GeoChallenger.Services.Interfaces.DTO.Users;
using GeoChallenger.Web.Api.Models.Challenges;
using GeoChallenger.Services.Settings;
using GeoChallenger.Web.Api.Models.Media;
using GeoChallenger.Web.Api.Models.Pois;
using GeoChallenger.Web.Api.Models.Routes;
using GeoChallenger.Web.Api.Models.Users;

namespace GeoChallenger.Web.Api.Config
{
    public class MapperConfig
    {
        private static readonly ApplicationSettings ApplicationSettings = SettingsFactory.GetApplicationSettings();

        // Now all our challenges has the same reward.
        private const int DEFAULT_CHALLENGE_REWARD = 5;
        private const string MEDIA_CONTROLLER_ENDPOINT = "media";

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
            config.CreateMap<PoiDto, PoiReadViewModel>()
                .ForMember(dst => dst.Media, opt => opt.Ignore());

            config.CreateMap<SearchPoiResultDto, PoiPreviewViewModel>();

            config.CreateMap<PoiDto, PoiPreviewViewModel>();

            config.CreateMap<PoiMediaDto, PoiMediaReadViewModel>()
                .ForMember(dst => dst.MediaUrl, opt => opt.Ignore())
                .AfterMap((src, dst) => {
                    dst.MediaUrl = $"{ApplicationSettings.ServerUrl}/{MEDIA_CONTROLLER_ENDPOINT}/{src.MediaType}/{src.MediaName}/".ToLower();
                });

            config.CreateMap<RouteDto, RouteReadViewModel>();

            config.CreateMap<AccountTypeDto, AccountTypeViewModel>();

            config.CreateMap<UserDto, UserReadViewModel>();

            config.CreateMap<ChallengeDto, ChallengeReadViewModel>();

            config.CreateMap<MediaTypeDto, MediaTypeViewModel>();

            config.CreateMap<MediaUploadResultDto, MediaUploadResultViewModel>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.ContentType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dst => dst.MediaUrl, opt => opt.Ignore())
                .AfterMap((src, dst) => {
                    dst.MediaUrl = $"{ApplicationSettings.ServerUrl}/{MEDIA_CONTROLLER_ENDPOINT}/{src.MediaType}/{src.Name}/".ToLower();
                });
        }

        private static void MapFromViewModelsToContracts(IMapperConfiguration config)
        {
            config.CreateMap<PoiUpdateViewModel, PoiUpdateDto>();

            config.CreateMap<PoiMediaUpdateViewModel, PoiMediaUpdateDto>();

            config.CreateMap<AccountTypeViewModel, AccountTypeDto>();

            config.CreateMap<RouteUpdateViewModel, RouteUpdateDto>();

            config.CreateMap<ChallengeUpdateViewModel, ChallengeUpdateDto>()
                .ForMember(dst => dst.PointsReward, opt => opt.UseValue(DEFAULT_CHALLENGE_REWARD));
        }
    }
}