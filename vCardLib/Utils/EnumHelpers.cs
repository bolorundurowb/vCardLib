using System;
using vCardLib.Constants;
using vCardLib.Enums;

namespace vCardLib.Utils
{
    internal static class EnumHelpers
    {
        public static EmailAddressType ParseEmailType(string typeString)
        {
            if (EmailAddressTypeConstants.Internet.Equals(typeString, StringComparison.OrdinalIgnoreCase))
                return EmailAddressType.Internet;

            if (EmailAddressTypeConstants.Home.Equals(typeString, StringComparison.OrdinalIgnoreCase))
                return EmailAddressType.Home;

            if (EmailAddressTypeConstants.Work.Equals(typeString, StringComparison.OrdinalIgnoreCase))
                return EmailAddressType.Work;

            if (EmailAddressTypeConstants.Aol.Equals(typeString, StringComparison.OrdinalIgnoreCase))
                return EmailAddressType.Aol;

            if (EmailAddressTypeConstants.IbmMail.Equals(typeString, StringComparison.OrdinalIgnoreCase))
                return EmailAddressType.IbmMail;

            if (EmailAddressTypeConstants.AppleLink.Equals(typeString, StringComparison.OrdinalIgnoreCase))
                return EmailAddressType.Applelink;

            return EmailAddressType.None;
        }
    }
}