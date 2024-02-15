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
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var (telephoneNumber, extension) = SplitOutExtension(value);

        if (metadata.Length == 0)
            return new TelephoneNumber(telephoneNumber, extension: extension);

        TelephoneNumberType? type = null;
        int? preference = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
            {
                if (string.IsNullOrWhiteSpace(data))
                    continue;

                var typeGroup = data!.Split(',');

                foreach (var individualType in typeGroup)
                {
                    var phoneType = individualType.ParseTelephoneNumberType();

                    if (phoneType.HasValue)
                        type = type.HasValue ? type.Value | phoneType : phoneType;
                }
            }
            else if (key.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
                preference = 1;
        }

        return new TelephoneNumber(telephoneNumber, type, extension, preference);
    }

    public TelephoneNumber Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var (telephoneNumber, extension) = SplitOutExtension(value);

        if (metadata.Length == 0)
            return new TelephoneNumber(telephoneNumber, extension: extension);

        TelephoneNumberType? type = null;
        int? preference = null;
        string? _value = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
            {
                if (string.IsNullOrWhiteSpace(data))
                    continue;

                var typeGroup = data!.Split(',');

                foreach (var individualType in typeGroup)
                {
                    var phoneType = individualType.ParseTelephoneNumberType();

                    if (phoneType.HasValue)
                        type = type.HasValue ? type.Value | phoneType : phoneType;
                }
            }
            else if (key.EqualsIgnoreCase(FieldKeyConstants.ValueKey))
                _value = data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
                if (!string.IsNullOrWhiteSpace(data) && int.TryParse(data, out var pref))
                    preference = pref;
        }

        return new TelephoneNumber(telephoneNumber, type, extension, preference, _value);
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
