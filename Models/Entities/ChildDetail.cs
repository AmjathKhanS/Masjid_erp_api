using PersonalDetailsAPI.Models.Enums;

namespace PersonalDetailsAPI.Models.Entities;

public class ChildDetail : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Qualification { get; set; } = string.Empty;
    public MaritalStatus MaritalStatus { get; set; }
    public string BloodGroup { get; set; } = string.Empty;
    public bool IsPhysicallyChallenged { get; set; }

    // Foreign Key
    public int PersonalDetailId { get; set; }
    public PersonalDetail PersonalDetail { get; set; } = null!;
}
