using FluentValidation;
using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Validation
{
    public class SensorReadingCreateDtoValidator : AbstractValidator<SensorReadingCreateDto>
    {
        public SensorReadingCreateDtoValidator()
        {
            RuleFor(x => x.GreenhouseId).GreaterThan(0);
            RuleFor(x => x.AirTemp).InclusiveBetween(-50, 80);
            RuleFor(x => x.AirHum).InclusiveBetween(0, 100);
            RuleFor(x => x.SoilHum).InclusiveBetween(0, 100);
            RuleFor(x => x.SoilTemp).InclusiveBetween(-20, 80);
            RuleFor(x => x.LightLevel).GreaterThanOrEqualTo(0);
        }
    }
}
