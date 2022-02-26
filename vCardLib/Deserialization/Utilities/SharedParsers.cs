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

    public static AddressType? ParseAddressType(string input)
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
}