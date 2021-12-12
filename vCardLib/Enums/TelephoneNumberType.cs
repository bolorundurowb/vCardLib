using System;

namespace vCardLib.Enums
{
    /// <summary>
    /// Various types of numbers stored in a vcard
    /// </summary>
    [Flags]
    public enum TelephoneNumberType
    {
        None = 0,

        Voice = 1,

        Text = 2,

        Fax = 4,

        Cell = 8,

        Video = 16,

        Pager = 32,

        TextPhone = 64,

        Home = 128,

        MainNumber = 256,

        Work = 512,

        BBS = 1024,

        Modem = 2048,

        Car = 4096,

        ISDN = 8192,
        
        Pref = 16384
    }
}