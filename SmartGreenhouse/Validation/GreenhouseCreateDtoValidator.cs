using FluentValidation;
using SmartGreenhouse.Models.DTOs;

namespace SmartGreenhouse.Validation
{
    public class GreenhouseCreateDtoValidator: AbstractValidator<GreenhouseCreateDto>
    {
        public GreenhouseCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.Season)
                .MaximumLength(50);
            RuleFor(x => x.Location)
                .MaximumLength(200);
            RuleFor(x => x.Length)
                .GreaterThan(0);
            RuleFor(x => x.Width)
                .GreaterThan(0);
            RuleFor(x => x.Height)
                .GreaterThan(0);
            RuleFor(x => x.UserId)
                .GreaterThan(0);
        }
    }
}
