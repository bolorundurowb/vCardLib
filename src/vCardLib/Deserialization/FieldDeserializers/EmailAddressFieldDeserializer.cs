using System.Linq;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class EmailAddressFieldDeserializer : IV2FieldDeserializer<EmailAddress>,
    IV3FieldDeserializer<EmailAddress>, IV4FieldDeserializer<EmailAddress>
{
    public static string FieldKey => "EMAIL";

    public EmailAddress Read(string input)
    {
        var (parameters, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        EmailAddressType? type = null;
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
                var emailType = val.ParseEmailAddressType();
                if (emailType.HasValue)
                    type = type.HasValue ? type.Value | emailType : emailType;
            }

            else if (key != null && key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey) && val.EqualsIgnoreCase("QUOTED-PRINTABLE"))
                isQuotedPrintable = true;
        }

        if (isQuotedPrintable) value = SharedParsers.DecodeQuotedPrintable(value);

        return new EmailAddress(value, type, preference);
    }

    EmailAddress IV4FieldDeserializer<EmailAddress>.Read(string input)
    {
        var (parameters, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        EmailAddressType? type = null;
        int? preference = null;
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
                var emailType = val.ParseEmailAddressType();
                if (emailType.HasValue)
                    type = type.HasValue ? type.Value | emailType : emailType;
            }

            if (key != null && key.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
            {
                if (int.TryParse(val, out var pref))
                    preference = pref;
            }
            else if (key != null && key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey) && val.EqualsIgnoreCase("QUOTED-PRINTABLE"))
                isQuotedPrintable = true;
        }

        if (isQuotedPrintable) value = SharedParsers.DecodeQuotedPrintable(value);

        return new EmailAddress(value, type, preference);
    }
}
