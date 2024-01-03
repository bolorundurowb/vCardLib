using System;
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
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        if (metadata.Length == 0)
            return new EmailAddress(value);

        EmailAddressType? type = null;
        int? preference = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey) && data != null)
            {
                var emailTypes = data.Split(',');

                type = emailTypes
                    .Select(parsedType => parsedType.ParseEmailAddressType())
                    .Aggregate(type, (current, emailType) => current.HasValue ? current.Value | emailType : emailType);
            }
            else if (key.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
                preference = 1;
        }

        return new EmailAddress(value, type, preference);
    }

    EmailAddress IV4FieldDeserializer<EmailAddress>.Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        if (metadata.Length == 0)
            return new EmailAddress(value);

        EmailAddressType? type = null;
        int? preference = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey) && data != null)
            {
                var emailTypes = data.Split(',');

                type = emailTypes
                    .Select(parsedType => parsedType.ParseEmailAddressType())
                    .Aggregate(type, (current, emailType) => current.HasValue ? current.Value | emailType : emailType);
            }
            else if (key.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
                if (!string.IsNullOrWhiteSpace(data) && int.TryParse(data, out var pref))
                    preference = pref;
        }

        return new EmailAddress(value, type, preference);
    }
}
