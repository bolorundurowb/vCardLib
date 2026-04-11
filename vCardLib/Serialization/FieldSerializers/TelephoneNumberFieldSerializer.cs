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

internal sealed class TelephoneNumberFieldSerializer : IV2FieldSerializer<TelephoneNumber>,
    IV3FieldSerializer<TelephoneNumber>, IV4FieldSerializer<TelephoneNumber>
{
    public string FieldKey => "TEL";

    string IV2FieldSerializer<TelephoneNumber>.Write(TelephoneNumber data)
    {
        return WriteInternal(data, false);
    }

    public string Write(TelephoneNumber data)
    {
        return WriteInternal(data, true);
    }

    private string WriteInternal(TelephoneNumber data, bool v4Style)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != TelephoneNumberType.None)
        {
            var telephoneNumberTypes = EnumCache<TelephoneNumberType>.Values
                .Where(x => data.Type.HasFlag(x) && x != TelephoneNumberType.None)
                .ToArray();

            if (telephoneNumberTypes.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);
                if (v4Style)
                {
                    builder.AppendFormat("{0}=", FieldKeyConstants.TypeKey);
                    var typesList = telephoneNumberTypes.Select(t => t.DecomposeTelephoneNumberType()).ToList();
                    builder.Append(string.Join(FieldKeyConstants.ConcatenationDelimiter.ToString(), typesList));
                }
                else
                {
                    for (var i = 0; i < telephoneNumberTypes.Length; i++)
                    {
                        if (i > 0)
                            builder.Append(FieldKeyConstants.MetadataDelimiter);

                        builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey,
                            telephoneNumberTypes[i].DecomposeTelephoneNumberType());
                    }
                }
            }
        }

        if (v4Style && !string.IsNullOrWhiteSpace(data.Value))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.ValueKey, data.Value);
        }

        if (data.Preference.HasValue)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            if (v4Style)
                builder.AppendFormat("{0}={1}", FieldKeyConstants.PreferenceKey, data.Preference);
            else
                builder.Append(FieldKeyConstants.PreferenceKey);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Number);

        return builder.ToString();
    }
}
