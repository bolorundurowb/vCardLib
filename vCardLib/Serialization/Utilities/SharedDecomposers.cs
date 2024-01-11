using System;
using vCardLib.Enums;

namespace vCardLib.Serialization.Utilities;

internal static class SharedDecomposers
{
    public static string DecomposeAddressType(this AddressType input)
    {
        return input switch
        {
            AddressType.Domestic => "dom",
            AddressType.Home => "home",
            AddressType.International => "intl",
            AddressType.Parcel => "parcel",
            AddressType.Postal => "postal",
            AddressType.Work => "work",
            _ => throw new ArgumentException()
        };
    }

    public static string DecomposeEmailAddressType(this EmailAddressType input)
    {
        return input switch
        {
            EmailAddressType.Work => "work",
            EmailAddressType.Internet => "internet",
            EmailAddressType.Home => "home",
            EmailAddressType.Aol => "aol",
            EmailAddressType.Applelink => "applelink",
            EmailAddressType.IbmMail => "ibmmail",
            EmailAddressType.Preferred => "pref",
            _ => throw new ArgumentException()
        };
    }

    public static string DecomposeBiologicalSex(this BiologicalSex input)
    {
        return input switch
        {
            BiologicalSex.Male => "M",
            BiologicalSex.Female => "F",
            BiologicalSex.Other => "O",
            BiologicalSex.None => "N",
            BiologicalSex.Unknown => "U",
            _ => throw new ArgumentException()
        };
    }
}
