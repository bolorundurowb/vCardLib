using vCardLib.Enums;

namespace vCardLib.Models;

public struct TelephoneNumber
{
    /// <summary>
    /// The Number
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// The tel value type
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// The telephone number extension
    /// </summary>
    public string? Extension { get; set; }

    /// <summary>
    /// The number type
    /// </summary>
    public TelephoneNumberType Type { get; set; }

    /// <summary>
    /// Indicates the telephone numbers preference level (lower values mean a higher preference)
    /// </summary>
    public int? Preference { get; set; }

    public TelephoneNumber(string number, TelephoneNumberType? telephoneNumberType = null, string? extension = null, int? preference = null, string? value = null)
    {
        Number = number;
        Value = value;
        Extension = extension;
        Preference = preference;
        Type = telephoneNumberType ?? TelephoneNumberType.None;
    }
}