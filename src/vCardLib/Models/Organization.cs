namespace vCardLib.Models;

public struct Organization(string name, string? primaryUnit, string? secondaryUnit)
{
    /// <summary>
    /// The organization name
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// The organization primary unit
    /// </summary>
    public string? PrimaryUnit { get; set; } = primaryUnit;

    /// <summary>
    /// The organization secondary unit
    /// </summary>
    public string? SecondaryUnit { get; set; } = secondaryUnit;
}