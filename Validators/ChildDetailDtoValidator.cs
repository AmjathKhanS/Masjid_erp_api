using FluentValidation;
using PersonalDetailsAPI.Models.DTOs;

namespace PersonalDetailsAPI.Validators;

public class ChildDetailDtoValidator : AbstractValidator<ChildDetailDto>
{
    public ChildDetailDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Child name is required")
            .MaximumLength(100).WithMessage("Child name cannot exceed 100 characters");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender value");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Child date of birth is required")
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past");

        RuleFor(x => x.Qualification)
            .NotEmpty().WithMessage("Qualification is required")
            .MaximumLength(50).WithMessage("Qualification cannot exceed 50 characters");

        RuleFor(x => x.BloodGroup)
            .NotEmpty().WithMessage("Blood group is required")
            .MaximumLength(10).WithMessage("Blood group cannot exceed 10 characters");
    }
}
