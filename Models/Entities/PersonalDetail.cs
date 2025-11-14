using PersonalDetailsAPI.Models.Enums;

namespace PersonalDetailsAPI.Models.Entities;

public class PersonalDetail : BaseEntity
{
    // MANDATORY FIELDS (Required by user)
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

    // Occupation Details (Optional)
    public OccupationType? Occupation { get; set; }
    public string? OccupationDetail { get; set; }
    public IncomeRange? MonthlyIncome { get; set; }

    // Contact Details (Optional)
    public string? EmailId { get; set; }

    // Father Details (Optional)
    public string? FatherName { get; set; }
    public string? FatherOccupation { get; set; }
    public int? FatherDeathYear { get; set; }

    // Mother Details (Optional)
    public string? MotherName { get; set; }
    public string? MotherOccupation { get; set; }
    public int? MotherDeathYear { get; set; }

    // Wife Details (Optional)
    public int? NumberOfWives { get; set; }

    // Children Details (Optional)
    public int? NumberOfChildren { get; set; }

    // Feedback (Optional)
    public string? Feedback { get; set; }

    // Navigation Properties
    public ICollection<WifeDetail> WifeDetails { get; set; } = new List<WifeDetail>();
    public ICollection<ChildDetail> ChildDetails { get; set; } = new List<ChildDetail>();
}
