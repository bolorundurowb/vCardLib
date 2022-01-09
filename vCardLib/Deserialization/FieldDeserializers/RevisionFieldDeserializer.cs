using System;
using System.Globalization;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class RevisionFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<DateTime?>,
    IV3FieldDeserializer<DateTime?>, IV4FieldDeserializer<DateTime?>
{
    public string FieldKey => "REV";

    public DateTime? Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        var value = input.Substring(separatorIndex + 1)
            .Replace("-", "").Trim();

        const string format = "yyyyMMddTHHmmssZ";
        IFormatProvider provider = new CultureInfo("en-US");
        if (DateTime.TryParseExact(value, format, provider, DateTimeStyles.AssumeUniversal, out var revision))
            return revision;

        return null;
    }
}