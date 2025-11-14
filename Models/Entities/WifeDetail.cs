using PersonalDetailsAPI.Models.Enums;

namespace PersonalDetailsAPI.Models.Entities;

public class WifeDetail : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Occupation { get; set; } = string.Empty;
    public string Native { get; set; } = string.Empty;
    public string Caste { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public MaritalStatus MaritalStatus { get; set; }

    // Foreign Key
    public int PersonalDetailId { get; set; }
    public PersonalDetail PersonalDetail { get; set; } = null!;
}
