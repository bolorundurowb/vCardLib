using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal abstract class TextFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>, IV4FieldSerializer<string>
{
    public abstract string FieldKey { get; }

    string IV2FieldSerializer<string>.Write(string data)
        // vCard 2.1 does not support backslash escaping.
        => FormatField(data);

    string IV3FieldSerializer<string>.Write(string data)
        => FormatField(Escape(data));

    string IV4FieldSerializer<string>.Write(string data)
        => FormatField(Escape(data));

    private string FormatField(string value)
        => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{value}";

    private static string Escape(string data) => ValueEscaper.Escape(data);
}