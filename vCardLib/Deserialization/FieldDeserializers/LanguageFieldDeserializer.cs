using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class LanguageFieldDeserializer : IV2FieldDeserializer<Language?>,
    IV3FieldDeserializer<Language?>, IV4FieldDeserializer<Language?>
{
    public static string FieldKey => "LANG";

    public Language? Read(string input) => null;

    Language? IV4FieldDeserializer<Language?>.Read(string input)
    {
        var (metadata, locale) = DataSplitHelpers.SplitLine(FieldKey, input);

        if (metadata.Length == 0)
            return new Language(locale);

        int? preference = null;
        string? type = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
                type = data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
                preference = int.TryParse(data, out var parsedPref) ? parsedPref : 1;
        }

        return new Language(locale, preference, type);
    }
}
