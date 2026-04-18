using System.Collections.Generic;
using System.Text;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class TelephoneNumberFieldSerializer : IV2FieldSerializer<TelephoneNumber>,
    IV3FieldSerializer<TelephoneNumber>, IV4FieldSerializer<TelephoneNumber>
{
    public string FieldKey => "TEL";

    string IV2FieldSerializer<TelephoneNumber>.Write(TelephoneNumber data)
    {
        var types = data.Type.DecomposeTelephoneNumberTypes();
        var parameters = SerializationHelpers.FormatParameters(vCardVersion.v2, types, data.Preference);
        return $"{FieldKey}{parameters}{FieldKeyConstants.SectionDelimiter}{data.Number}";
    }

    string IV3FieldSerializer<TelephoneNumber>.Write(TelephoneNumber data)
    {
        var types = data.Type.DecomposeTelephoneNumberTypes();
        var parameters = SerializationHelpers.FormatParameters(vCardVersion.v3, types, data.Preference);
        return $"{FieldKey}{parameters}{FieldKeyConstants.SectionDelimiter}{data.Number}";
    }

    public string Write(TelephoneNumber data)
    {
        var types = data.Type.DecomposeTelephoneNumberTypes();
        var extra = new List<(string Key, string Value)>();
        if (!string.IsNullOrWhiteSpace(data.Value))
        {
            extra.Add((FieldKeyConstants.ValueKey, data.Value!));
        }
        var parameters = SerializationHelpers.FormatParameters(vCardVersion.v4, types, data.Preference, extra);
        return $"{FieldKey}{parameters}{FieldKeyConstants.SectionDelimiter}{data.Number}";
    }
}
