using System;

namespace vCardLib.Enums;

/// <summary>
/// Various types of URLs stored in a vCard
/// </summary>
[Flags]
public enum UrlType
{
    Work = 1,

    Home = 2,

    Blog = 4,

    Profile = 8,

    Url = 16
}