using System.Collections.Generic;
using System.IO;
using System.Text;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.Utilities;

internal static class DeserializerHelpers
{
    static Dictionary<vCardVersion, Dictionary<>>
    public static IEnumerable<string[]> SplitContent(string vcardContent)
    {
        using var reader = new StringReader(vcardContent);
        var response = new List<string>();
    
        while (reader.ReadLine()?.Trim() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.EqualsIgnoreCase("END:VCARD"))
            {
                yield return response.ToArray();
            }
            else if (line.EqualsIgnoreCase("BEGIN:VCARD"))
            {
                response.Clear();
            }
            else if (line.EndsWithIgnoreCase("BEGIN:VCARD"))
            {
                var nested = new StringBuilder(line);
                while (reader.ReadLine()?.Trim() is { } nestedLine && !nestedLine.EqualsIgnoreCase("END:VCARD"))
                {
                    nested.AppendLine(nestedLine);
                }
                response.Add(nested.ToString());
            }
            else
            {
                response.Add(line);
            }
        }
    }

    public static vCard Convert(string[] vcardContent)
    {
        
    }
}
