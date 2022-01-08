namespace vCardLib.Models;

public struct Organization
{
    public string Name { get; set; }

    public string PrimaryUnit { get; set; }

    public string SecondaryUnit { get; set; }

    public Organization(string name, string primaryUnit = null, string secondaryUnit = null)
    {
        Name = name;
        PrimaryUnit = primaryUnit;
        SecondaryUnit = secondaryUnit;
    }
}