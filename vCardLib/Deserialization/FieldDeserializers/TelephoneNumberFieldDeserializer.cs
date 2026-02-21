using System.Linq;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class TelephoneNumberFieldDeserializer : IV2FieldDeserializer<TelephoneNumber>,
    IV3FieldDeserializer<TelephoneNumber>, IV4FieldDeserializer<TelephoneNumber>
{
    public static string FieldKey => "TEL";

    TelephoneNumber IV2FieldDeserializer<TelephoneNumber>.Read(string input)
    {
        return ReadInternal(input, false);
    }

    public TelephoneNumber Read(string input)
    {
        return ReadInternal(input, true);
    }

    private TelephoneNumber ReadInternal(string input, bool numericPreference)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var (telephoneNumber, extension) = SplitOutExtension(value);

        var parameters = VCardParameters.Parse(metadata);

        var type = ParameterInterpreters.ParseTypeFlags<TelephoneNumberType>(parameters, SharedParsers.ParseTelephoneNumberType, FieldKeyConstants.PreferenceKey, FieldKeyConstants.ValueKey);
        var preference = ParameterInterpreters.ParsePreference(parameters, numericPreference);
        var valueType = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.ValueKey);

        return new TelephoneNumber(telephoneNumber, type, extension, preference, valueType);
    }

    private (string, string?) SplitOutExtension(string input)
    {
        // HACK: in case the telephone number is in uri format
        input = input.ToUpperInvariant()
            .Replace(FieldKey, string.Empty)
            .TrimStart(FieldKeyConstants.SectionDelimiter);

        var phoneNumberParts = input.Split(FieldKeyConstants.MetadataDelimiter);

        if (phoneNumberParts.Length < 2)
            return (input, null);

        var phoneNumber = phoneNumberParts[0];

        foreach (var phoneNumberMetadata in phoneNumberParts.Skip(1))
        {
            var datum = phoneNumberMetadata.Split('=');

            if (datum.Any() && datum.First().EqualsIgnoreCase("EXT"))
                return (phoneNumber, datum.Skip(1).First());
        }

        return (phoneNumber, null);
    }
}
