using System.Collections.Generic;
using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Enums;

namespace vCardLib.Serialization.Utilities;

internal static class SerializationHelpers
{
    public static string FormatParameters(vCardVersion version, IEnumerable<string> types, int? preference = null, IEnumerable<(string Key, string Value)>? extraParams = null)
    {
        var builder = new StringBuilder();
        var typeList = types.ToList();

        if (version == vCardVersion.v2)
        {
            // v2.1: Prefer bare types or repeated TYPE=
            foreach (var type in typeList)
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);
                builder.Append(type.ToUpperInvariant());
            }

            if (preference.HasValue)
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);
                builder.Append(FieldKeyConstants.PreferenceKey);
            }
        }
        else
        {
            // v3.0 / v4.0: TYPE=home,work
            if (typeList.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);
                builder.Append(FieldKeyConstants.TypeKey);
                builder.Append(FieldKeyConstants.KeyValueDelimiter);
                builder.Append(string.Join(FieldKeyConstants.ConcatenationDelimiter.ToString(), typeList));
            }

            if (preference.HasValue)
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);
                builder.Append(FieldKeyConstants.PreferenceKey);
                if (version == vCardVersion.v4)
                {
                    builder.Append(FieldKeyConstants.KeyValueDelimiter);
                    builder.Append(preference.Value);
                }
            }
        }

        if (extraParams != null)
        {
            foreach (var (key, value) in extraParams)
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);
                builder.Append(key);
                builder.Append(FieldKeyConstants.KeyValueDelimiter);
                builder.Append(value);
            }
        }

        return builder.ToString();
    }
}
