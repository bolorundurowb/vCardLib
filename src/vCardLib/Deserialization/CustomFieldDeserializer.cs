using System.Collections.Generic;
using vCardLib.Constants;
using vCardLib.Models;

namespace vCardLib.Deserialization;

internal sealed class CustomFieldDeserializer : IFieldDeserializer
{
    public string FieldKey => "UNKNOWN";

    public void Deserialize(string line, vCard card)
    {
        var separatorIndex = line.LastIndexOf(FieldKeyConstants.SectionDelimiter);
        var key = line.Substring(0, separatorIndex).Trim();
        var value = line.Substring(separatorIndex + 1).Trim();
        card.CustomFields.Add(new KeyValuePair<string, string>(key, value));
    }
}
