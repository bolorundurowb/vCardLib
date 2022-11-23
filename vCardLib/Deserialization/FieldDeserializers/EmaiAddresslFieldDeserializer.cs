using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;
using vCardLib.Utilities;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class EmaiAddresslFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<EmailAddress>,
    IV3FieldDeserializer<EmailAddress>, IV4FieldDeserializer<EmailAddress>
{
    public string FieldKey => "EMAIL";

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

            if (key.EqualsIgnoreCase("TYPE"))
            {
                var emailType = data?.ParseEmailAddressType();

                if (emailType.HasValue)
                    type = type.HasValue ? type.Value | emailType : emailType;
            }
            else if (key.EqualsIgnoreCase("PREF"))
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

            if (key.EqualsIgnoreCase("TYPE"))
            {
                var emailType = data?.ParseEmailAddressType();

                if (emailType.HasValue)
                    type = type.HasValue ? type.Value | emailType : emailType;
            }
            else if (key.EqualsIgnoreCase("PREF"))
                if (!string.IsNullOrWhiteSpace(data) && int.TryParse(data, out var pref))
                    preference = pref;
        }

        return new EmailAddress(value, type, preference);
    }
}
