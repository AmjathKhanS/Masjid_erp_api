using PersonalDetailsAPI.Models.Enums;

namespace PersonalDetailsAPI.Models.DTOs;

public class UpdatePersonalDetailDto
{
    // MANDATORY FIELDS
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public ResidentialStatus ResidentialStatus { get; set; }
    public string Address { get; set; } = string.Empty;

    // OPTIONAL FIELDS
    public string? AlternateNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? CasteGroup { get; set; }
    public string? AadharNumber { get; set; }
    public string? Qualification { get; set; }
    public DateTime? MarriageDate { get; set; }
    public string? BloodGroup { get; set; }
    public OccupationType? Occupation { get; set; }
    public string? OccupationDetail { get; set; }
    public IncomeRange? MonthlyIncome { get; set; }
    public string? EmailId { get; set; }
    public string? FatherName { get; set; }
    public string? FatherOccupation { get; set; }
    public int? FatherDeathYear { get; set; }
    public string? MotherName { get; set; }
    public string? MotherOccupation { get; set; }
    public int? MotherDeathYear { get; set; }
    public int? NumberOfWives { get; set; }
    public int? NumberOfChildren { get; set; }
    public string? Feedback { get; set; }
    public List<WifeDetailDto>? WifeDetails { get; set; }
    public List<ChildDetailDto>? ChildDetails { get; set; }
}
