using FluentValidation;
using PersonalDetailsAPI.Models.DTOs;

namespace PersonalDetailsAPI.Validators;

public class CreatePersonalDetailDtoValidator : AbstractValidator<CreatePersonalDetailDto>
{
    public CreatePersonalDetailDtoValidator()
    {
        // MANDATORY FIELDS VALIDATION
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits");

        RuleFor(x => x.ResidentialStatus)
            .IsInEnum().WithMessage("Valid residential status is required");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        // OPTIONAL FIELDS VALIDATION (only validate if provided)
        RuleFor(x => x.AlternateNumber)
            .Matches(@"^\d{10}$").WithMessage("Alternate number must be exactly 10 digits")
            .When(x => !string.IsNullOrEmpty(x.AlternateNumber));

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past")
            .When(x => x.DateOfBirth.HasValue);

        RuleFor(x => x.AadharNumber)
            .Matches(@"^\d{12}$").WithMessage("Aadhar number must be exactly 12 digits")
            .When(x => !string.IsNullOrEmpty(x.AadharNumber));

        RuleFor(x => x.EmailId)
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.EmailId));

        RuleFor(x => x.CasteGroup)
            .MaximumLength(50).WithMessage("Caste/Group cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.CasteGroup));

        RuleFor(x => x.Qualification)
            .MaximumLength(50).WithMessage("Qualification cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Qualification));

        RuleFor(x => x.BloodGroup)
            .MaximumLength(10).WithMessage("Blood group cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.BloodGroup));

        RuleFor(x => x.OccupationDetail)
            .MaximumLength(200).WithMessage("Occupation detail cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.OccupationDetail));

        RuleFor(x => x.FatherName)
            .MaximumLength(100).WithMessage("Father's name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.FatherName));

        RuleFor(x => x.FatherOccupation)
            .MaximumLength(100).WithMessage("Father's occupation cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.FatherOccupation));

        RuleFor(x => x.MotherName)
            .MaximumLength(100).WithMessage("Mother's name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.MotherName));

        RuleFor(x => x.MotherOccupation)
            .MaximumLength(100).WithMessage("Mother's occupation cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.MotherOccupation));

        RuleFor(x => x.NumberOfWives)
            .InclusiveBetween(1, 2).WithMessage("Number of wives must be between 1 and 2")
            .When(x => x.NumberOfWives.HasValue);

        RuleFor(x => x.NumberOfChildren)
            .InclusiveBetween(0, 4).WithMessage("Number of children must be between 0 and 4")
            .When(x => x.NumberOfChildren.HasValue);

        RuleFor(x => x.Feedback)
            .MaximumLength(1000).WithMessage("Feedback cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Feedback));

        // Validate wife details if provided
        RuleFor(x => x.WifeDetails)
            .Must((dto, list) => list == null || list.Count == 0 || (dto.NumberOfWives.HasValue && list.Count == dto.NumberOfWives.Value))
            .WithMessage("Number of wife details must match NumberOfWives")
            .When(x => x.WifeDetails != null && x.WifeDetails.Any());

        RuleForEach(x => x.WifeDetails)
            .SetValidator(new WifeDetailDtoValidator())
            .When(x => x.WifeDetails != null);

        // Validate child details if provided
        RuleFor(x => x.ChildDetails)
            .Must((dto, list) => list == null || list.Count == 0 || (dto.NumberOfChildren.HasValue && list.Count == dto.NumberOfChildren.Value))
            .WithMessage("Number of child details must match NumberOfChildren")
            .When(x => x.ChildDetails != null && x.ChildDetails.Any());

        RuleForEach(x => x.ChildDetails)
            .SetValidator(new ChildDetailDtoValidator())
            .When(x => x.ChildDetails != null);
    }
}
