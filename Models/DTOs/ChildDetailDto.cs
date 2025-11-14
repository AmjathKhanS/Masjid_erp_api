using PersonalDetailsAPI.Models.Enums;

namespace PersonalDetailsAPI.Models.DTOs;

public class ChildDetailDto
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Qualification { get; set; } = string.Empty;
    public MaritalStatus MaritalStatus { get; set; }
    public string BloodGroup { get; set; } = string.Empty;
    public bool IsPhysicallyChallenged { get; set; }
}
