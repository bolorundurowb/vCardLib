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

        if (EmailAddressTypeConstants.Preferred.EqualsIgnoreCase(type))
            return EmailAddressType.Preferred;

        return null;
    }
        
    public static TelephoneNumberType? ParseTelephoneNumberType(this string type)
    {
        if (TelephoneNumberTypeConstants.Voice.EqualsIgnoreCase(type))
            return TelephoneNumberType.Voice;

        if (TelephoneNumberTypeConstants.Text.EqualsIgnoreCase(type))
            return TelephoneNumberType.Text;

        if (TelephoneNumberTypeConstants.Fax.EqualsIgnoreCase(type))
            return TelephoneNumberType.Fax;

        if (TelephoneNumberTypeConstants.Cell.EqualsIgnoreCase(type))
            return TelephoneNumberType.Cell;

        if (TelephoneNumberTypeConstants.Video.EqualsIgnoreCase(type))
            return TelephoneNumberType.Video;

        if (TelephoneNumberTypeConstants.Pager.EqualsIgnoreCase(type))
            return TelephoneNumberType.Pager;

        if (TelephoneNumberTypeConstants.TextPhone.EqualsIgnoreCase(type))
            return TelephoneNumberType.TextPhone;

        if (TelephoneNumberTypeConstants.Home.EqualsIgnoreCase(type))
            return TelephoneNumberType.Home;

        if (TelephoneNumberTypeConstants.MainNumber.EqualsIgnoreCase(type))
            return TelephoneNumberType.MainNumber;

        if (TelephoneNumberTypeConstants.Work.EqualsIgnoreCase(type))
            return TelephoneNumberType.Work;

        if (TelephoneNumberTypeConstants.Bbs.EqualsIgnoreCase(type))
            return TelephoneNumberType.BBS;

        if (TelephoneNumberTypeConstants.Modem.EqualsIgnoreCase(type))
            return TelephoneNumberType.Modem;

        if (TelephoneNumberTypeConstants.Car.EqualsIgnoreCase(type))
            return TelephoneNumberType.Car;

        if (TelephoneNumberTypeConstants.Isdn.EqualsIgnoreCase(type))
            return TelephoneNumberType.ISDN;

        if (TelephoneNumberTypeConstants.Pcs.EqualsIgnoreCase(type))
            return TelephoneNumberType.PCS;

        if (TelephoneNumberTypeConstants.Preferred.EqualsIgnoreCase(type))
            return TelephoneNumberType.Preferred;

        return null;
    }
}