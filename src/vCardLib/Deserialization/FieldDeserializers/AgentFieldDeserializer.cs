using System.Text;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class AgentFieldDeserializer : IV2FieldDeserializer<string>,
    IV3FieldDeserializer<string>, IV4FieldDeserializer<string?>
{
    public static string FieldKey => "AGENT";

    public string Read(string input)
    {
        var colonIndex = FindValueColon(input);
        var value = colonIndex >= 0 ? input.Substring(colonIndex + 1) : input;

        value = value.Trim();

        const string valuePreamble = "VALUE=";
        if (value.StartsWith(valuePreamble))
            value = value.Substring(valuePreamble.Length);

        return UnescapeVCard(value);
    }

    string? IV4FieldDeserializer<string?>.Read(string input) => null;

    private static int FindValueColon(string input)
    {
        var inQuotes = false;
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '"')
                inQuotes = !inQuotes;
            else if (input[i] == ':' && !inQuotes)
                return i;
        }
        return -1;
    }

    private static string UnescapeVCard(string input)
    {
        var sb = new StringBuilder(input.Length);
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '\\' && i + 1 < input.Length)
            {
                switch (input[i + 1])
                {
                    case 'n':
                    case 'N':
                        sb.Append('\n');
                        i++;
                        break;
                    case ';':
                        sb.Append(';');
                        i++;
                        break;
                    case ',':
                        sb.Append(',');
                        i++;
                        break;
                    case '\\':
                        sb.Append('\\');
                        i++;
                        break;
                    default:
                        sb.Append(input[i]);
                        break;
                }
            }
            else
            {
                sb.Append(input[i]);
            }
        }
        return sb.ToString();
    }
}
