using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;
using vCardLib.Utilities;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class UrlFieldDeserializer : IV2FieldDeserializer<Url>, IV3FieldDeserializer<Url>,
    IV4FieldDeserializer<Url>
{
    public static string FieldKey => "URL";

    public Url Read(string input)
    {
        return ReadInternal(input, false);
    }

    Url IV4FieldDeserializer<Url>.Read(string input)
    {
        return ReadInternal(input, true);
    }

    private static Url ReadInternal(string input, bool numericPreference)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var parameters = VCardParameters.Parse(metadata);

        var type = ParameterInterpreters.ParseTypeFlags<UrlType>(parameters, SharedParsers.ParseUrlType,
            FieldKeyConstants.LabelKey, FieldKeyConstants.CharacterSetKey, FieldKeyConstants.LanguageSetKey,
            FieldKeyConstants.PreferenceKey, FieldKeyConstants.MediaTypeKey, FieldKeyConstants.MediaTypeAltKey);
        var pref = ParameterInterpreters.ParsePreference(parameters, numericPreference);
        var label = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.LabelKey);
        var mimeType = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.MediaTypeKey)
                       ?? ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.MediaTypeAltKey);
        var language = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.LanguageSetKey);
        var charset = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.CharacterSetKey);

        return new Url(value, type, pref, label, mimeType, language, charset);
    }
}