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
                .GreaterThan(0).WithMessage("Довжина має бути більше 0.");
            RuleFor(x => x.Width)
                .GreaterThan(0).WithMessage("Ширина має бути більше 0.");
            RuleFor(x => x.Height)
                .GreaterThan(0).WithMessage("Висота має бути більше 0.");
            RuleFor(x => x.PlantIds)
                .NotNull().WithMessage("Список рослин не може бути порожнім.")
                .Must(list => list.Count > 0).WithMessage("Необхідно вибрати хоча б одну рослину.")
                .Must(list => list.Count <= 3).WithMessage("Можна вибрати не більше трьох рослин.");
        }
    }
}
