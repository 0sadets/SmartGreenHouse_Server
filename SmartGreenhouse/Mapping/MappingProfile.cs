﻿using AutoMapper;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;

namespace SmartGreenhouse.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<LoginDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)); 

            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Greenhouses, opt => opt.MapFrom(src => src.Greenhouses)) 
                .ForMember(dest => dest.UserSettings, opt => opt.MapFrom(src => src.UserSettings)); 


            CreateMap<GreenhouseCreateDto, Greenhouse>();
            //CreateMap<GreenhouseUpdateDto, Greenhouse>();
            CreateMap<Greenhouse, GreenhouseReadDto>();

            CreateMap<Plant, PlantReadDto>();
            CreateMap<Plant, PlantExampleReadDto>();

            CreateMap<GreenhouseCreateDto, Greenhouse>()
                .ForMember(dest => dest.Plants, opt => opt.Ignore());

            CreateMap<Greenhouse, GreenhouseReadDto>()
                .ForMember(dest => dest.Plants, opt => opt.MapFrom(src => src.Plants));

            CreateMap<UserSetting, CreateUserSettingsDto>().ReverseMap();

            CreateMap<SensorReadingCreateDto, SensorReading>();
            CreateMap<UserSetting, UserSettingsDto>();
            CreateMap<GHStatusCreateDto, GreenhouseStatusRecord>();

            CreateMap<GreenhouseUpdateDto, Greenhouse>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                 .ForMember(dest => dest.UserId, opt => opt.Ignore())
                 .ForMember(dest => dest.Plants, opt => opt.Ignore());
            CreateMap<DeviceState, DeviceStateDto>()
            .ForMember(dest => dest.ghId, opt => opt.MapFrom(src => src.GreenhouseId.ToString()));
            CreateMap<DeviceStateDto, DeviceState>()
                .ForMember(dest => dest.GreenhouseId, opt => opt.MapFrom(src => int.Parse(src.ghId)));

            CreateMap<AppUser, UserReadDto>();
            CreateMap<UserUpdateDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.UserName)))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Email)));

        }
    }
}
