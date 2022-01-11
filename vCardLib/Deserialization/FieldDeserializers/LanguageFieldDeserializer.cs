using System;
using System.Linq;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class LanguageFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Language?>,
    IV3FieldDeserializer<Language?>, IV4FieldDeserializer<Language?>
{
    public string FieldKey => "LANG";

    public Language? Read(string input) => null;

    Language? IV4FieldDeserializer<Language?>.Read(string input)
    {
        var parts = input.Split(':');

        // the lang entry must have two parts
        if (parts.Length != 2)
            return null;

        int? preference = null;
        string? type = null;

        var metadata = parts[0].Split(';');

        var typeMetadatum = metadata.First(x => x.StartsWith("TYPE", StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrWhiteSpace(typeMetadatum))
        {
            var typeParts = typeMetadatum.Split('=');

            if (typeParts.Length == 2)
                type = typeParts[1];
        }

        var preferenceMetadatum = metadata.First(x => x.StartsWith("PREF", StringComparison.CurrentCultureIgnoreCase));

        if (!string.IsNullOrWhiteSpace(preferenceMetadatum))
        {
            var prefParts = preferenceMetadatum.Split('=');

            if (prefParts.Length == 2 && int.TryParse(prefParts[1], out var pref))
                preference = pref;
        }

        return new Language(parts[1], preference, type);
    }
}