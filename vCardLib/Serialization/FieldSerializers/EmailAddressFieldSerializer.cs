using System;
using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class EmailAddressFieldSerializer : IV2FieldSerializer<EmailAddress>, IV3FieldSerializer<EmailAddress>,
    IV4FieldSerializer<EmailAddress>
{
    public string FieldKey => "EMAIL";

    public string? Write(EmailAddress data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != EmailAddressType.None)
        {
            var emailTypes = Enum.GetValues(typeof(EmailAddressType))
                .Cast<EmailAddressType>()
                .Where(x => data.Type.HasFlag(x) && x != EmailAddressType.None)
                .ToArray();

            if (emailTypes.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);

                foreach (var emailType in emailTypes)
                    builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, emailType.DecomposeEmailAddressType());
            }
        }

        if (data.Preference.HasValue)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(FieldKeyConstants.PreferenceKey);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Value);

        return builder.ToString();
    }

    string? IV4FieldSerializer<EmailAddress>.Write(EmailAddress data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != EmailAddressType.None)
        {
            var emailTypes = Enum.GetValues(typeof(EmailAddressType))
                .Cast<EmailAddressType>()
                .Where(x => data.Type.HasFlag(x) && x != EmailAddressType.None)
                .ToArray();

            if (emailTypes.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);

                foreach (var emailType in emailTypes)
                    builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, emailType.DecomposeEmailAddressType());
            }
        }

        if (data.Preference.HasValue)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.PreferenceKey, data.Preference.Value);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Value);

        return builder.ToString();
    }
}
