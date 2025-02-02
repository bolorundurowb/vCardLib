using System;
using System.Collections.Concurrent;
using System.Globalization;
using vCardLib.Constants;
using vCardLib.Enums;

namespace vCardLib.Deserialization.Utilities;

internal static class SharedParsers
{
    static readonly ConcurrentDictionary<string, EmailAddressType> EmailTypeMap
        = new(StringComparer.OrdinalIgnoreCase)
        {
            ["internet"] = EmailAddressType.Internet,
            ["home"] = EmailAddressType.Home,
            ["work"] = EmailAddressType.Work,
            ["aol"] = EmailAddressType.Aol,
            ["ibmmail"] = EmailAddressType.IbmMail,
            ["applelink"] = EmailAddressType.Applelink,
            ["pref"] = EmailAddressType.Preferred,
        };

    static readonly ConcurrentDictionary<string, AddressType> AddressTypeMap
        = new(StringComparer.OrdinalIgnoreCase)
        {
            ["dom"] = AddressType.Domestic,
            ["home"] = AddressType.Home,
            ["intl"] = AddressType.International,
            ["parcel"] = AddressType.Parcel,
            ["postal"] = AddressType.Postal,
            ["work"] = AddressType.Work,
        };

    private static readonly ConcurrentDictionary<string, TelephoneNumberType> TelephoneNumberTypeMap
        = new(StringComparer.OrdinalIgnoreCase)
        {
            ["voice"] = TelephoneNumberType.Voice,
            ["text"] = TelephoneNumberType.Text,
            ["fax"] = TelephoneNumberType.Fax,
            ["cell"] = TelephoneNumberType.Cell,
            ["video"] = TelephoneNumberType.Video,
            ["pager"] = TelephoneNumberType.Pager,
            ["textphone"] = TelephoneNumberType.TextPhone,
            ["home"] = TelephoneNumberType.Home,
            ["main-number"] = TelephoneNumberType.MainNumber,
            ["work"] = TelephoneNumberType.Work,
            ["bbs"] = TelephoneNumberType.BBS,
            ["modem"] = TelephoneNumberType.Modem,
            ["car"] = TelephoneNumberType.Car,
            ["isdn"] = TelephoneNumberType.ISDN,
            ["pcs"] = TelephoneNumberType.PCS,
            ["pref"] = TelephoneNumberType.Preferred,
        };

    public static DateTime? ParseDate(string input)
    {
        IFormatProvider provider = new CultureInfo("en-US");
        const string dateFormat = "yyyyMMdd";
        const string timeFormat = "hhmm";
        const string dateTimeFormat = "yyyyMMddTHHmmssZ";

        input = input.Replace(FieldKeyConstants.KeyValueDelimiter.ToString(), string.Empty)
            .TrimStart(FieldKeyConstants.SectionDelimiter)
            .TrimStart(FieldKeyConstants.MetadataDelimiter)
            .TrimStart();

        if (DateTime.TryParseExact(input, dateFormat, provider, DateTimeStyles.AssumeUniversal, out var date))
            return date.ToUniversalTime();

        if (DateTime.TryParseExact(input, dateTimeFormat, provider, DateTimeStyles.AssumeUniversal, out var dateTime))
            return dateTime.ToUniversalTime();

        if (TimeSpan.TryParseExact(input, timeFormat, provider, TimeSpanStyles.None, out var time))
            return DateTime.MinValue.ToUniversalTime() + time;

        return null;
    }

    public static AddressType? ParseAddressType(this string input) =>
        AddressTypeMap.TryGetValue(input, out var result) ? result : null;

    public static EmailAddressType? ParseEmailAddressType(this string type) =>
        EmailTypeMap.TryGetValue(type, out var result) ? result : null;

    public static TelephoneNumberType? ParseTelephoneNumberType(this string type) => TelephoneNumberTypeMap.TryGetValue(type, out var result) ? result : null;
}