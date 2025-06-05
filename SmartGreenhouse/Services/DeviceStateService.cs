using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;
using SmartGreenhouse.Repositories;
using System.IO.Ports;

namespace SmartGreenhouse.Services
{
    public class DeviceStateService : IDeviceStateService
    {
        private readonly IRepository<DeviceState> _repository;
        private readonly IRepository<Greenhouse> _ghRepository;
        private readonly IGreenhouseService _greenhouseService;
        private readonly IMapper _mapper;

        public DeviceStateService(
            IRepository<DeviceState> deviceRepository,
           IRepository<Greenhouse> ghRepository,
            IGreenhouseService greenhouseService,
            IMapper mapper)

        {
            _repository = deviceRepository;
            _ghRepository = ghRepository;
            _greenhouseService = greenhouseService;
            _mapper = mapper;
        }
        public async Task<bool> UpdateDeviceStateAsync(DeviceUpdateRequest request)
        {
            int? greenhouseId = _greenhouseService.GetAssignedGreenhouseId("ARDUINO-001");

            if (greenhouseId == null)
                throw new Exception("Greenhouse not found.");

            var lastState = (await _repository.GetAllAsync())
                .Where(ds => ds.GreenhouseId == greenhouseId)
                .OrderByDescending(ds => ds.Timestamp)
                .FirstOrDefault();

            var newState = new DeviceState
            {
                GreenhouseId = greenhouseId.Value,
                Timestamp = DateTime.UtcNow,
                FanStatus = request.DeviceType == "fan" ? request.NewState : lastState?.FanStatus ?? false,
                DoorStatus = request.DeviceType == "door" ? request.NewState : lastState?.DoorStatus ?? false
            };

            await _repository.AddAsync(newState);
            await _repository.SaveAsync();

            SendCommandToArduino(request.DeviceType, request.NewState);

            return true;
        }

        private void SendCommandToArduino(string deviceType, bool newState)
        {
            using (var port = new SerialPort("COM5", 9600))
            {
                port.Open();
                string command = deviceType switch
                {
                    "fan" => newState ? "FAN_ON" : "FAN_OFF",
                    "door" => newState ? "DOOR_OPEN" : "DOOR_CLOSE",
                    _ => throw new ArgumentException("Invalid device type")
                };
                port.WriteLine(command);
            }
        }
        public async Task<DeviceStateDto?> GetLastDeviceStateAsync(int greenhouseId)
        {
            var lastState = (await _repository.GetAllAsync())
                .Where(ds => ds.GreenhouseId == greenhouseId)
                .OrderByDescending(ds => ds.Timestamp)
                .FirstOrDefault();

            if (lastState == null)
                return null;

            return _mapper.Map<DeviceStateDto>(lastState);
        }

    }
}
