namespace vCardLib.Models;

public struct Name
{
    /// <summary>
    /// The Family name or Surname of the contact
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// The Firstname or Given name of the contact
    /// </summary>
    public string? GivenName { get; set; }

    /// <summary>
    /// Any additional name of the contact
    /// </summary>
    public string? AdditionalNames { get; set; }

    /// <summary>
    /// The honorific prefix of the contact
    /// </summary>
    public string? HonorificPrefix { get; set; }

    /// <summary>
    /// The honorific suffix of the contact
    /// </summary>
    public string? HonorificSuffix { get; set; }

    public Name(string? familyName, string? givenName, string? additionalNames, string? honorificPrefix,
        string? honorificSuffix)
    {
        FamilyName = familyName;
        GivenName = givenName;
        AdditionalNames = additionalNames;
        HonorificPrefix = honorificPrefix;
        HonorificSuffix = honorificSuffix;
    }
}