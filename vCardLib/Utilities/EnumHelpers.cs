using System;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Extensions;

namespace vCardLib.Utilities;

internal static class EnumHelpers
{
    public static EmailAddressType? ParseEmailAddressType(this string type)
    {
        if (EmailAddressTypeConstants.Internet.EqualsIgnoreCase(type))
            return EmailAddressType.Internet;

        if (EmailAddressTypeConstants.Home.EqualsIgnoreCase(type))
            return EmailAddressType.Home;

        if (EmailAddressTypeConstants.Work.EqualsIgnoreCase(type))
            return EmailAddressType.Work;

        if (EmailAddressTypeConstants.Aol.EqualsIgnoreCase(type))
            return EmailAddressType.Aol;

        if (EmailAddressTypeConstants.IbmMail.EqualsIgnoreCase(type))
            return EmailAddressType.IbmMail;

        if (EmailAddressTypeConstants.AppleLink.EqualsIgnoreCase(type))
            return EmailAddressType.Applelink;

        return null;
    }
        
    public static TelephoneNumberType ParseTelephoneType(string typeString)
    {
        if (TelephoneNumberTypeConstants.Voice.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Voice;

        if (TelephoneNumberTypeConstants.Text.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Text;

        if (TelephoneNumberTypeConstants.Fax.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Fax;

        if (TelephoneNumberTypeConstants.Cell.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Cell;

        if (TelephoneNumberTypeConstants.Video.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Video;

        if (TelephoneNumberTypeConstants.Pager.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Pager;

        if (TelephoneNumberTypeConstants.TextPhone.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.TextPhone;

        if (TelephoneNumberTypeConstants.Home.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Home;

        if (TelephoneNumberTypeConstants.MainNumber.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.MainNumber;

        if (TelephoneNumberTypeConstants.Work.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Work;

        if (TelephoneNumberTypeConstants.Bbs.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.BBS;

        if (TelephoneNumberTypeConstants.Modem.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Modem;

        if (TelephoneNumberTypeConstants.Car.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Car;

        if (TelephoneNumberTypeConstants.Isdn.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.ISDN;

        if (TelephoneNumberTypeConstants.Pref.Equals(typeString, StringComparison.OrdinalIgnoreCase))
            return TelephoneNumberType.Pref;

        return TelephoneNumberType.None;
    }
}