using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class UnknownFieldDeserializer : IUnknownFieldSerializer
{
    public static string Key => "UNKNOWN";

    public (string, string) Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        var key = input.Substring(0, separatorIndex).Trim();
        var value = input.Substring(separatorIndex + 1).Trim();
        return (key, value);
    }
}
