using AutoMapper;
using PromomashInc.DataAccess.Models;
using PromomashInc.EntitiesDto;
using PromomashInc.EntitiesDto.Command;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace PromomashInc.Mapper
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

            CreateMap<User, CreateUserCommand>()
                .ReverseMap();
            CreateMap<UserDto, CreateUserCommand>()
                .ReverseMap();
        }


    }
}
