using System;
using System.Globalization;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class AnniversaryFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<DateTime?>,
    IV3FieldDeserializer<DateTime?>, IV4FieldDeserializer<DateTime?>
{
    public string FieldKey => "ANNIVERSARY";

    public DateTime? Read(string input) => null;

    DateTime? IV4FieldDeserializer<DateTime?>.Read(string input)
    {
        input = input.ToUpper().Replace(FieldKey, string.Empty);
        return SharedDeserializers.ParseDate(input);
    }
}