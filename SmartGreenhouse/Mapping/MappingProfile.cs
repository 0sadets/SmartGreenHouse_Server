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


        }
    }
}
