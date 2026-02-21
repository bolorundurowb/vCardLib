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
        var parameters = VCardParameters.Parse(metadata);

        var type = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.TypeKey);
        var preference = ParameterInterpreters.ParsePreference(parameters, true);

        return new Language(locale, preference, type);
    }
}
