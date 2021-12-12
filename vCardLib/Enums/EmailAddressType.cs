using System;

namespace vCardLib.Enums
{
    /// <summary>
    /// Various email address types in a vCard
    /// </summary>
    [Flags]
    public enum EmailAddressType
    {
        None = 0,

        Work = 1,

        Internet = 2,

        Home = 4,

        Aol = 8,

        Applelink = 16,

        IbmMail = 32,
        
        Pref = 64
    }
}