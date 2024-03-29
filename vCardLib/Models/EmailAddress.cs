﻿using vCardLib.Enums;

namespace vCardLib.Models;

public struct EmailAddress
{
    /// <summary>
    /// The email address value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// The email address type
    /// </summary>
    public EmailAddressType Type { get; set; }

    /// <summary>
    /// Indicates the email address' preference level (lower  values mean a higher preference)
    /// </summary>
    public int? Preference { get; set; }

    public EmailAddress(string value, EmailAddressType? type = null, int? preference = null)
    {
        Value = value;
        Preference = preference;
        Type = type ?? EmailAddressType.None;
    }
}
