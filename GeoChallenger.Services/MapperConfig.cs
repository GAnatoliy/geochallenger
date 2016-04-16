﻿using AutoMapper;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Services.Extensions;
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
            config.CreateMap<Poi, PoiDto>()
                .ForMember(dst => dst.Latitude, opt => opt.Ignore())
                .ForMember(dst => dst.Longitude, opt => opt.Ignore())
                .AfterMap((src, dst) => {
                    if (src.Location?.Latitude != null && src.Location.Longitude.HasValue) {
                        dst.Latitude = src.Location.Latitude.Value;
                        dst.Longitude = src.Location.Longitude.Value;
                    }
                });
        }

        private static void MapFromContractsToDomains(IMapperConfiguration config)
        {
            config.CreateMap<PoiUpdateDto, Poi>()
                .ForMember(dst => dst.PoiId, opt => opt.Ignore())
                .ForMember(dst => dst.Location, opt => opt.Ignore())
                .AfterMap((src, dst) => {
                    dst.Location = GeoExtensions.CreateLocationPoint(src.Latitude, src.Longitude);
                });
        }
    }
}