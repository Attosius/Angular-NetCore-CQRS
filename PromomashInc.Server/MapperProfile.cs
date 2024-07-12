
using AutoMapper;
using PromomashInc.Server.Context;
using PromomashInc.Server.Dto;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PromomashInc.Server
{
    public class AutoMapperConfig
    {
        public static IConfigurationProvider Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CommonMapperProfile());
            });
            return config;
        }


    }
    public class CommonMapperProfile : Profile
    {
        public CommonMapperProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();
            CreateMap<Country, CountryDto>()
                .ReverseMap();
            CreateMap<Province, ProvinceDto>()
                .ReverseMap();
        }


    }
}
