using vCardLib.Enums;

namespace vCardLib.Models;

public struct TelephoneNumber(
    string number,
    TelephoneNumberType? telephoneNumberType = null,
    string? extension = null,
    int? preference = null,
    string? value = null)
{
    /// <summary>
    /// The Number
    /// </summary>
    public string Number { get; set; } = number;

    /// <summary>
    /// The tel value type
    /// </summary>
    public string? Value { get; set; } = value;

    /// <summary>
    /// The telephone number extension
    /// </summary>
    public string? Extension { get; set; } = extension;

    /// <summary>
    /// The number type
    /// </summary>
    public TelephoneNumberType Type { get; set; } = telephoneNumberType ?? TelephoneNumberType.None;

    /// <summary>
    /// Indicates the telephone numbers preference level (lower values mean a higher preference)
    /// </summary>
    public int? Preference { get; set; } = preference;
}