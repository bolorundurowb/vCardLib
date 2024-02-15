namespace vCardLib.Models;

public struct Organization
{
    /// <summary>
    /// The organization name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The organization primary unit
    /// </summary>
    public string? PrimaryUnit { get; set; }

    /// <summary>
    /// The organization secondary unit
    /// </summary>
    public string? SecondaryUnit { get; set; }

    public Organization(string name, string? primaryUnit, string? secondaryUnit)
    {
        Name = name;
        PrimaryUnit = primaryUnit;
        SecondaryUnit = secondaryUnit;
    }
}