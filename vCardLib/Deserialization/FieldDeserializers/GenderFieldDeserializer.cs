using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class GenderFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Gender?>,
    IV3FieldDeserializer<Gender?>, IV4FieldDeserializer<Gender>
{
    public string FieldKey => "GENDER";

    public Gender? Read(string input) => null;

    Gender IV4FieldDeserializer<Gender>.Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        var value = input.Substring(separatorIndex + 1).Trim();

        BiologicalSex? sex = null;
        string? genderIdentity = null;

        var parts = value.Split(';');
        var partsLength = parts.Length;

        if (partsLength > 0)
            sex = parts[0].ToUpperInvariant() switch
            {
                "M" => BiologicalSex.Male,
                "F" => BiologicalSex.Female,
                "O" => BiologicalSex.Other,
                "N" => BiologicalSex.None,
                "U" => BiologicalSex.Unknown,
                _ => null
            };

        if (partsLength > 1)
            genderIdentity = parts[1];

        return new Gender(sex, genderIdentity);
    }
}