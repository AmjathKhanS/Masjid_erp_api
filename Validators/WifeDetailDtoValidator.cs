using FluentValidation;
using PersonalDetailsAPI.Models.DTOs;

namespace PersonalDetailsAPI.Validators;

public class WifeDetailDtoValidator : AbstractValidator<WifeDetailDto>
{
    public WifeDetailDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Wife name is required")
            .MaximumLength(100).WithMessage("Wife name cannot exceed 100 characters");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Wife date of birth is required")
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past");

        RuleFor(x => x.Occupation)
            .NotEmpty().WithMessage("Wife occupation is required")
            .MaximumLength(100).WithMessage("Occupation cannot exceed 100 characters");

        RuleFor(x => x.Native)
            .NotEmpty().WithMessage("Native place is required")
            .MaximumLength(100).WithMessage("Native place cannot exceed 100 characters");

        RuleFor(x => x.Caste)
            .NotEmpty().WithMessage("Caste is required")
            .MaximumLength(50).WithMessage("Caste cannot exceed 50 characters");

        RuleFor(x => x.Qualification)
            .NotEmpty().WithMessage("Qualification is required")
            .MaximumLength(50).WithMessage("Qualification cannot exceed 50 characters");

        RuleFor(x => x.BloodGroup)
            .NotEmpty().WithMessage("Blood group is required")
            .MaximumLength(10).WithMessage("Blood group cannot exceed 10 characters");
    }
}
