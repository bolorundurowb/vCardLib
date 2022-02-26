using System.Collections.Generic;
using System.Text.RegularExpressions;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class LabelFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Label>, IV3FieldDeserializer<Label>, IV4FieldDeserializer<Label?>
{
    public string FieldKey => "LABEL";

    public Label Read(string input)
    {
        input = input.Replace(FieldKey, string.Empty);
        var parts = input.Split(':');

        var label = new Label(parts.Length > 1 ? parts[1] : null);
        label.Types.AddRange(ParseTypes(parts[0]));

        return label;
    }

    Label? IV4FieldDeserializer<Label?>.Read(string input) => null;

    private static IEnumerable<AddressType> ParseTypes(string part)
    {
        
    }
}