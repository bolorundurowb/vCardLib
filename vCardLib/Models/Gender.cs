using vCardLib.Enums;

namespace vCardLib.Models;

public struct Gender
{
    public BiologicalSex? Sex { get; set; }
    
    public string? GenderIdentity { get; set; }

    public Gender(BiologicalSex? sex, string? genderIdentity)
    {
        Sex = sex;
        GenderIdentity = genderIdentity;
    }
}