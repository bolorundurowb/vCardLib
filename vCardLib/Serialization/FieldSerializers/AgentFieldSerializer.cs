using System;
using System.Text;
using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class AgentFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>, IV4FieldSerializer<string>
{
    public string FieldKey => "AGENT";

    public string Write(string data)
    {
        var builder = new StringBuilder(FieldKey);

        if (Uri.IsWellFormedUriString(data, UriKind.Absolute))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append("VALUE=uri");
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data);

        return builder.ToString();
    }
}
