﻿using System;
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
}