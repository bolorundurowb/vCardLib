namespace vCardLib.Models;

public struct Name(
    string? familyName,
    string? givenName,
    string? additionalNames,
    string? honorificPrefix,
    string? honorificSuffix)
{
    /// <summary>
    /// The Family name or Surname of the contact
    /// </summary>
    public string? FamilyName { get; set; } = familyName;

    /// <summary>
    /// The Firstname or Given name of the contact
    /// </summary>
    public string? GivenName { get; set; } = givenName;

    /// <summary>
    /// Any additional name of the contact
    /// </summary>
    public string? AdditionalNames { get; set; } = additionalNames;

    /// <summary>
    /// The honorific prefix of the contact
    /// </summary>
    public string? HonorificPrefix { get; set; } = honorificPrefix;

    /// <summary>
    /// The honorific suffix of the contact
    /// </summary>
    public string? HonorificSuffix { get; set; } = honorificSuffix;
}