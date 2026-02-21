using System;
using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

using vCardLib.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class EmailAddressFieldSerializer : IV2FieldSerializer<EmailAddress>, IV3FieldSerializer<EmailAddress>,
    IV4FieldSerializer<EmailAddress>
{
    public string FieldKey => "EMAIL";

    public string? Write(EmailAddress data)
    {
        return WriteInternal(data, false);
    }

    string? IV4FieldSerializer<EmailAddress>.Write(EmailAddress data)
    {
        return WriteInternal(data, true);
    }

    private string WriteInternal(EmailAddress data, bool v4Style)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != EmailAddressType.None)
        {
            var emailTypes = EnumCache<EmailAddressType>.Values
                .Where(x => data.Type.HasFlag(x) && x != EmailAddressType.None)
                .ToArray();

            if (emailTypes.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);

                if (v4Style)
                {
                    builder.AppendFormat("{0}=", FieldKeyConstants.TypeKey);
                    var typesList = emailTypes.Select(t => t.DecomposeEmailAddressType()).ToList();
                    builder.Append(string.Join(FieldKeyConstants.ConcatenationDelimiter.ToString(), typesList));
                }
                else
                {
                    foreach (var emailType in emailTypes)
                        builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, emailType.DecomposeEmailAddressType());
                }
            }
        }

        if (data.Preference.HasValue)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            if (v4Style)
                builder.AppendFormat("{0}={1}", FieldKeyConstants.PreferenceKey, data.Preference.Value);
            else
                builder.Append(FieldKeyConstants.PreferenceKey);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Value);

        return builder.ToString();
    }
}
