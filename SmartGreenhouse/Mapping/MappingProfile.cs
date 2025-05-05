using AutoMapper;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            CreateMap<GreenhouseCreateDto, Greenhouse>();
            CreateMap<GreenhouseUpdateDto, Greenhouse>();
            CreateMap<Greenhouse, GreenhouseReadDto>();

            CreateMap<Plant, PlantReadDto>();

            CreateMap<GreenhouseCreateDto, Greenhouse>()
                .ForMember(dest => dest.Plants, opt => opt.Ignore());

            CreateMap<Greenhouse, GreenhouseReadDto>()
                .ForMember(dest => dest.Plants, opt => opt.MapFrom(src => src.Plants));

            CreateMap<UserSetting, CreateUserSettingsDto>();




        }
    }
}
