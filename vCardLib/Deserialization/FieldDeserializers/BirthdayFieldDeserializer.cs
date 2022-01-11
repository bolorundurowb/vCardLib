using System;
using System.Globalization;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class BirthdayFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<DateTime?>,
    IV3FieldDeserializer<DateTime?>, IV4FieldDeserializer<DateTime?>
{
    public string FieldKey => "BDAY";

    public DateTime? Read(string input)
    {
        IFormatProvider provider = new CultureInfo("en-US");
        const string dateFormat = "yyyyMMdd";
        const string timeFormat = "--HHmm";
        const string dateTimeFormat = "yyyyMMddTHHmmssZ";

        input = input.ToUpper().Replace(FieldKey, string.Empty);
        input = input.TrimStart(':').TrimStart(';').TrimStart();

        if (DateTime.TryParseExact(input, dateFormat, provider, DateTimeStyles.AssumeUniversal, out var date))
            return date;

        if (DateTime.TryParseExact(input, dateTimeFormat, provider, DateTimeStyles.AssumeUniversal, out var dateTime))
            return dateTime;

        if (TimeSpan.TryParseExact(input, timeFormat, provider, TimeSpanStyles.None, out var time))
            return DateTime.MinValue + time;

        return null;
    }
}