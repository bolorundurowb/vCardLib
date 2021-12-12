using System;

namespace vCardLib.Enums
{
    /// <summary>
    /// Various email address types in a vCard
    /// </summary>
    [Flags]
    public enum EmailType
    {
        None = 0,

        Work = 1,

        Internet = 2,

        Home = 4,

        AOL = 8,

        Applelink = 16,

        IBMMail = 32
    }
}