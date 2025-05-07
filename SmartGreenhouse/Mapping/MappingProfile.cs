using AutoMapper;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            // Мапінг для реєстрації користувача
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // Мапінг для входу користувача
            CreateMap<LoginDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)); 

            // Мапінг для передачі даних користувача в DTO 
            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Greenhouses, opt => opt.MapFrom(src => src.Greenhouses)) 
                .ForMember(dest => dest.UserSettings, opt => opt.MapFrom(src => src.UserSettings)); 


            CreateMap<GreenhouseCreateDto, Greenhouse>();
            CreateMap<GreenhouseUpdateDto, Greenhouse>();
            CreateMap<Greenhouse, GreenhouseReadDto>();

            CreateMap<Plant, PlantReadDto>();

            CreateMap<GreenhouseCreateDto, Greenhouse>()
                .ForMember(dest => dest.Plants, opt => opt.Ignore());

            CreateMap<Greenhouse, GreenhouseReadDto>()
                .ForMember(dest => dest.Plants, opt => opt.MapFrom(src => src.Plants));

            CreateMap<UserSetting, CreateUserSettingsDto>().ReverseMap();

            CreateMap<SensorReadingCreateDto, SensorReading>();



        }
    }
}
