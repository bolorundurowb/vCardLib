using System;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class BirthdayFieldDeserializer : IV2FieldDeserializer<DateTime?>,
    IV3FieldDeserializer<DateTime?>, IV4FieldDeserializer<DateTime?>
{
    public static string FieldKey => "BDAY";

    public DateTime? Read(string input)
    {
        var colonIndex = input.IndexOf(':');
        if (colonIndex >= 0)
            input = input.Substring(colonIndex + 1);
        return SharedParsers.ParseDate(input);
    }
}