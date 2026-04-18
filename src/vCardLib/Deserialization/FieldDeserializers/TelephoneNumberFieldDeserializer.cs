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
        var (parameters, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        TelephoneNumberType? type = null;
        int? preference = null;
        var isQuotedPrintable = false;

        foreach (var (key, val) in DataSplitHelpers.ParseParameters(parameters))
        {
            if (val.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
            {
                preference = 1;
            }

            if (key == null || key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
            {
                var phoneType = val.ParseTelephoneNumberType();
                if (phoneType.HasValue)
                    type = type.HasValue ? type.Value | phoneType : phoneType;
            }

            if (key != null && key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey) && val.EqualsIgnoreCase("QUOTED-PRINTABLE"))
                isQuotedPrintable = true;
        }

        if (isQuotedPrintable) value = SharedParsers.DecodeQuotedPrintable(value);
        var (telephoneNumber, extension) = SplitOutExtension(value);

        return new TelephoneNumber(telephoneNumber, type, extension, preference);
    }

    public TelephoneNumber Read(string input)
    {
        var (parameters, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        TelephoneNumberType? type = null;
        int? preference = null;
        string? _value = null;
        var isQuotedPrintable = false;

        foreach (var (key, val) in DataSplitHelpers.ParseParameters(parameters))
        {
            if (val.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
            {
                if (key == null || key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
                {
                    preference = 1;
                }
            }

            if (key == null || key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
            {
                var phoneType = val.ParseTelephoneNumberType();
                if (phoneType.HasValue)
                    type = type.HasValue ? type.Value | phoneType : phoneType;
            }

            if (key != null && key.EqualsIgnoreCase(FieldKeyConstants.ValueKey))
                _value = val;
            else if (key != null && key.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
            {
                if (int.TryParse(val, out var pref))
                    preference = pref;
            }
            else if (key != null && key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey) && val.EqualsIgnoreCase("QUOTED-PRINTABLE"))
                isQuotedPrintable = true;
        }

        if (isQuotedPrintable) value = SharedParsers.DecodeQuotedPrintable(value);
        var (telephoneNumber, extension) = SplitOutExtension(value);

        return new TelephoneNumber(telephoneNumber, type, extension, preference, _value);
    }

    private (string, string?) SplitOutExtension(string input)
    {
        // HACK: in case the telephone number is in uri format
        if (input.StartsWithIgnoreCase("tel:"))
        {
            // basic URI handling, could be more robust
            input = input.Substring(4);
        }

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
