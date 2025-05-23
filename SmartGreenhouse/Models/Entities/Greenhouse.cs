﻿using System.Text.Json.Serialization;

namespace SmartGreenhouse.Models.Entities
{
    public class Greenhouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Season { get; set; }
        public string Location { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<SensorReading> SensorReadings { get; set; }
        public ICollection<DeviceState> DeviceStates { get; set; }
        [JsonIgnore]
        public ICollection<UserSetting> UserSettings { get; set; }
        public ICollection<Plant> Plants { get; set; }


    }
}
