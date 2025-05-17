using AutoMapper;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;
using SmartGreenhouse.Repositories;

namespace SmartGreenhouse.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IRepository<UserSetting> _repository;
        private readonly IMapper _mapper;

        public UserSettingsService(IRepository<UserSetting> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public UserSettingsDto GetByGreenhouseId(int greenhouseId)
        {
            var setting = _repository.Get(s => s.GreenhouseId == greenhouseId).FirstOrDefault();

            return setting == null ? null : _mapper.Map<UserSettingsDto>(setting);
        }
    }

}
