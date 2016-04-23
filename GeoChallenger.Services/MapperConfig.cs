using System;
using AutoMapper;
using GeoChallenger.Database.Extensions;
using GeoChallenger.Domains.Challenges;
using GeoChallenger.Domains.Media;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Routes;
using GeoChallenger.Domains.Users;
using GeoChallenger.Search.Documents;
using GeoChallenger.Services.Interfaces.DTO.Challenges;
using GeoChallenger.Services.Interfaces.DTO.Media;
using GeoChallenger.Services.Interfaces.DTO.Pois;
using GeoChallenger.Services.Interfaces.DTO.Routes;
using GeoChallenger.Services.Interfaces.DTO.Users;
using GeoChallenger.Services.Providers.DTO;

namespace GeoChallenger.Services
{
    public class MapperConfig
    {
        public static void ConfigureMappings(IMapperConfiguration config)
        {
            MapFromDomainsToContracts(config);
            MapFromContractsToDomains(config);
            MapSearchDocuments(config);
        }

        private static void MapFromDomainsToContracts(IMapperConfiguration config)
        {
            config.CreateMap<Poi, PoiDto>()
                .ForMember(dst => dst.Latitude, opt => opt.Ignore())
                .ForMember(dst => dst.Longitude, opt => opt.Ignore())
                .AfterMap((src, dst) => {
                    if (src.Location?.Latitude != null && src.Location.Longitude.HasValue) {
                        dst.Latitude = src.Location.Latitude.Value;
                        dst.Longitude = src.Location.Longitude.Value;
                    }
                });

            config.CreateMap<PoiMedia, PoiMediaDto>();

            config.CreateMap<Route, RouteDto>();

            config.CreateMap<AccountType, AccountTypeDto>();

            config.CreateMap<Account, AccountDto>();

            config.CreateMap<User, UserDto>();

            config.CreateMap<Challenge, ChallengeDto>();

            config.CreateMap<MediaType, MediaTypeDto>();
        }

        private static void MapFromContractsToDomains(IMapperConfiguration config)
        {
            config.CreateMap<PoiUpdateDto, Poi>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Location, opt => opt.Ignore())
                .ForMember(dst => dst.ContentPreview, opt => opt.Ignore())
                .ForMember(dst => dst.Content, opt => opt.Ignore())
                .ForMember(dst => dst.IsDeleted, opt => opt.Ignore())
                .ForMember(dst => dst.OwnerId, opt => opt.Ignore())
                .ForMember(dst => dst.Owner, opt => opt.Ignore())
                .ForMember(dst => dst.Routes, opt => opt.Ignore())
                .ForMember(dst => dst.CreatedAtUtc, opt => opt.Ignore())
                .ForMember(dst => dst.Challengeses, opt => opt.Ignore())
                .ForMember(dst => dst.Checkins, opt => opt.Ignore())
                .ForMember(dst => dst.HashTags, opt => opt.Ignore())
                .AfterMap((src, dst) => {
                    dst.Location = GeoExtensions.CreateLocationPoint(src.Latitude, src.Longitude);
                });

            config.CreateMap<PoiMediaUpdateDto, PoiMedia>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.CreatedAtUtc, opt => opt.Ignore())
                .ForMember(dst => dst.UserId, opt => opt.Ignore())
                .ForMember(dst => dst.PoiId, opt => opt.Ignore())
                .ForMember(dst => dst.Poi, opt => opt.Ignore());

            config.CreateMap<RouteUpdateDto, Route>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.CreatedAtUtc, opt => opt.Ignore())
                .ForMember(dst => dst.IsDeleted, opt => opt.Ignore())
                .ForMember(dst => dst.UserId, opt => opt.Ignore())
                .ForMember(dst => dst.User, opt => opt.Ignore())
                .ForMember(dst => dst.Pois, opt => opt.Ignore());

            config.CreateMap<AccountTypeDto, AccountType>();

            config.CreateMap<SocialNetworkValidationData, Account>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.Uid, opt => opt.MapFrom(src => src.Uid))
                .ForMember(dst => dst.AttachedAtUtc, opt => opt.UseValue(DateTime.UtcNow))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dst => dst.UserId, opt => opt.Ignore())
                .ForMember(dst => dst.User, opt => opt.Ignore());

            config.CreateMap<ChallengeUpdateDto, Challenge>()
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForMember(dst => dst.CorrectAnswer, opt => opt.MapFrom(src => Challenge.SanitizeAnswer(src.CorrectAnswer)))
                .ForMember(dst => dst.CreatedAtUtc, opt => opt.Ignore())
                .ForMember(dst => dst.UpdatedAtUtc, opt => opt.Ignore())
                .ForMember(dst => dst.IsDeleted, opt => opt.Ignore())
                .ForMember(dst => dst.PoiId, opt => opt.Ignore())
                .ForMember(dst => dst.Poi, opt => opt.Ignore())
                .ForMember(dst => dst.CreatorId, opt => opt.Ignore())
                .ForMember(dst => dst.Creator, opt => opt.Ignore())
                .ForMember(dst => dst.Answers, opt => opt.Ignore());

            config.CreateMap<MediaTypeDto, MediaType>();
        }

        private static void MapSearchDocuments(IMapperConfiguration config)
        {
            config.CreateMap<Poi, PoiDocument>()
                .ForMember(m => m.Location, opt => opt.Ignore())
                .AfterMap((src, dst) => {
                    if (src.Location != null && src.Location?.Latitude != null && src.Location.Longitude.HasValue) {
                        dst.Location.Lat = src.Location.Latitude.Value;
                        dst.Location.Lon = src.Location.Longitude.Value;
                    }
                });
            config.CreateMap<PoiDocument, SearchPoiResultDto>()
                .ForMember(dst => dst.Latitude, opt => opt.MapFrom(src => src.Location.Lat))
                .ForMember(dst => dst.Longitude, opt => opt.MapFrom(src => src.Location.Lon));

            config.CreateMap<PoiDocument, PoiDto>()
                .ForMember(dst => dst.Latitude, opt => opt.MapFrom(src => src.Location.Lat))
                .ForMember(dst => dst.Longitude, opt => opt.MapFrom(src => src.Location.Lon));
        }
    }
}