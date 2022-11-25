using System;
using System.Globalization;
using vCardLib.Enums;
using vCardLib.Extensions;

namespace vCardLib.Deserialization.Utilities;

internal static class SharedParsers
{
    public static DateTime? ParseDate(string input)
    {
        IFormatProvider provider = new CultureInfo("en-US");
        const string dateFormat = "yyyyMMdd";
        const string timeFormat = "hhmm";
        const string dateTimeFormat = "yyyyMMddTHHmmssZ";

        input = input.Replace("-", string.Empty)
            .TrimStart(':')
            .TrimStart(';')
            .TrimStart();

        if (DateTime.TryParseExact(input, dateFormat, provider, DateTimeStyles.AssumeUniversal, out var date))
            return date.ToUniversalTime();

        if (DateTime.TryParseExact(input, dateTimeFormat, provider, DateTimeStyles.AssumeUniversal, out var dateTime))
            return dateTime.ToUniversalTime();

        if (TimeSpan.TryParseExact(input, timeFormat, provider, TimeSpanStyles.None, out var time))
            return DateTime.MinValue.ToUniversalTime() + time;

        return null;
    }

    public static AddressType? ParseAddressType(this string input)
    {
        if ("dom".EqualsIgnoreCase(input))
            return AddressType.Domestic;

        if ("home".EqualsIgnoreCase(input))
            return AddressType.Home;

        if ("intl".EqualsIgnoreCase(input))
            return AddressType.International;

        if ("parcel".EqualsIgnoreCase(input))
            return AddressType.Parcel;

        if ("postal".EqualsIgnoreCase(input))
            return AddressType.Postal;

        if ("work".EqualsIgnoreCase(input))
            return AddressType.Work;

        return null;
    }

    public static EmailAddressType? ParseEmailAddressType(this string type)
    {
        if ("internet".EqualsIgnoreCase(type))
            return EmailAddressType.Internet;

        if ("home".EqualsIgnoreCase(type))
            return EmailAddressType.Home;

        if ("work".EqualsIgnoreCase(type))
            return EmailAddressType.Work;

        if ("aol".EqualsIgnoreCase(type))
            return EmailAddressType.Aol;

        if ("ibmmail".EqualsIgnoreCase(type))
            return EmailAddressType.IbmMail;

        if ("applelink".EqualsIgnoreCase(type))
            return EmailAddressType.Applelink;

        if ("pref".EqualsIgnoreCase(type))
            return EmailAddressType.Preferred;

        return null;
    }
        
    public static TelephoneNumberType? ParseTelephoneNumberType(this string type)
    {
        if ("voice".EqualsIgnoreCase(type))
            return TelephoneNumberType.Voice;

        if ("text".EqualsIgnoreCase(type))
            return TelephoneNumberType.Text;

        if ("fax".EqualsIgnoreCase(type))
            return TelephoneNumberType.Fax;

        if ("cell".EqualsIgnoreCase(type))
            return TelephoneNumberType.Cell;

        if ("video".EqualsIgnoreCase(type))
            return TelephoneNumberType.Video;

        if ("pager".EqualsIgnoreCase(type))
            return TelephoneNumberType.Pager;

        if ("textphone".EqualsIgnoreCase(type))
            return TelephoneNumberType.TextPhone;

        if ("home".EqualsIgnoreCase(type))
            return TelephoneNumberType.Home;

        if ("main-number".EqualsIgnoreCase(type))
            return TelephoneNumberType.MainNumber;

        if ("work".EqualsIgnoreCase(type))
            return TelephoneNumberType.Work;

        if ("bbs".EqualsIgnoreCase(type))
            return TelephoneNumberType.BBS;

        if ("modem".EqualsIgnoreCase(type))
            return TelephoneNumberType.Modem;

        if ("car".EqualsIgnoreCase(type))
            return TelephoneNumberType.Car;

        if ("isdn".EqualsIgnoreCase(type))
            return TelephoneNumberType.ISDN;

        if ("pcs".EqualsIgnoreCase(type))
            return TelephoneNumberType.PCS;

        if ("pref".EqualsIgnoreCase(type))
            return TelephoneNumberType.Preferred;

        return null;
    }
}