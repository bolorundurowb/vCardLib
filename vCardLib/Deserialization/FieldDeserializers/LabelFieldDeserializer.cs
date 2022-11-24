using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class LabelFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Label>,
    IV3FieldDeserializer<Label>,
    IV4FieldDeserializer<Label?>
{
    public string FieldKey => "LABEL";

    public Label Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        var preamble = input.Substring(0, separatorIndex).Trim();
        var value = input.Substring(separatorIndex + 1).Trim();

        return new Label(Regex.Unescape(value), ParseTypes(preamble));
    }

    Label? IV4FieldDeserializer<Label?>.Read(string input) => null;

    private static AddressType? ParseTypes(string part)
    {
        AddressType? addressType = null;
        const string typeKey = "TYPE=";
        var typeGroups = part
            .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim());

        foreach (var typeGroup in typeGroups.Where(x => x.StartsWithIgnoreCase(typeKey)))
        {
            var value = typeGroup.Replace(typeKey, string.Empty);
            var types = value.Split(',');

            foreach (var type in types)
            {
                var parsedValue = SharedParsers.ParseAddressType(type);

                if (parsedValue == null)
                    continue;

                if (addressType.HasValue)
                    addressType |= parsedValue.Value;
                else
                    addressType = parsedValue.Value;
            }
        }

        return addressType;
    }
}
