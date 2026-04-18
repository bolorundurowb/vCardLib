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

    string? IV2FieldSerializer<EmailAddress>.Write(EmailAddress data)
    {
        var types = data.Type.DecomposeEmailAddressTypes();
        var parameters = SerializationHelpers.FormatParameters(vCardVersion.v2, types, data.Preference);
        return $"{FieldKey}{parameters}{FieldKeyConstants.SectionDelimiter}{data.Value}";
    }

    string? IV3FieldSerializer<EmailAddress>.Write(EmailAddress data)
    {
        var types = data.Type.DecomposeEmailAddressTypes();
        var parameters = SerializationHelpers.FormatParameters(vCardVersion.v3, types, data.Preference);
        return $"{FieldKey}{parameters}{FieldKeyConstants.SectionDelimiter}{data.Value}";
    }

    public string? Write(EmailAddress data) => ((IV3FieldSerializer<EmailAddress>)this).Write(data);

    string? IV4FieldSerializer<EmailAddress>.Write(EmailAddress data)
    {
        var types = data.Type.DecomposeEmailAddressTypes();
        var parameters = SerializationHelpers.FormatParameters(vCardVersion.v4, types, data.Preference);
        return $"{FieldKey}{parameters}{FieldKeyConstants.SectionDelimiter}{data.Value}";
    }
}
