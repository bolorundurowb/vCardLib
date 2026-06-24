using vCardLib.Enums;

namespace vCardLib.Models;

public struct EmailAddress(string value, EmailAddressType? type = null, int? preference = null)
{
    /// <summary>
    /// The email address value
    /// </summary>
    public string Value { get; set; } = value;

    /// <summary>
    /// The email address type
    /// </summary>
    public EmailAddressType Type { get; set; } = type ?? EmailAddressType.None;

    /// <summary>
    /// Indicates the email address' preference level (lower  values mean a higher preference)
    /// </summary>
    public int? Preference { get; set; } = preference;
}
