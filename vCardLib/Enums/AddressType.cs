using System;

namespace vCardLib.Enums;

/// <summary>
/// the delivery address type.
/// </summary>
[Flags]
public enum AddressType
{
    None = 0,

    Domestic = 1,

    International = 2,

    Postal = 4,

    Parcel = 8,

    Home = 16,

    Work = 32
}
