using System;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class BirthdayFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<DateTime?>,
    IV3FieldDeserializer<DateTime?>, IV4FieldDeserializer<DateTime?>
{
    public string FieldKey => "BDAY";

    public DateTime? Read(string input)
    {
        input = input.ToUpper().Replace(FieldKey, string.Empty);
        return SharedParsers.ParseDate(input);
    }
}