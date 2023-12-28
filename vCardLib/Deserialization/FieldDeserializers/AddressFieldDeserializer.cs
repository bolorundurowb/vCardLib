using System;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class AddressFieldDeserializer :  IV2FieldDeserializer<Address>,
    IV3FieldDeserializer<Address>, IV4FieldDeserializer<Address>
{
    public static string FieldKey => "ADR";

    public Address Read(string input)
    {
        var (metadata, values) = SplitLine(input);

        if (values.Length != 7)
            throw new Exception("Address parts incomplete.");

        if (metadata.Length == 0)
            return new Address(values[0], values[1], values[2], values[3], values[4], values[5], values[6]);

        AddressType? type = null;
        Geo? geo = null;
        string? label = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase("GEO"))
                geo = (new GeoFieldDeserializer() as IV4FieldDeserializer<Geo>)?.Read(data!);
            else if (key.EqualsIgnoreCase("LABEL"))
                label = data;
            else if (key.EqualsIgnoreCase("TYPE"))
                type = ParseAddressType(data!);
        }

        return new Address(values[0], values[1], values[2], values[3], values[4], values[5], values[6], type, label,
            geo);
    }

    private (string[], string[]) SplitLine(string input)
    {
        input = input.Replace(FieldKey, string.Empty)
            .TrimStart(FieldKeyConstants.MetadataDelimiter);
        var index = input.LastIndexOf(FieldKeyConstants.SectionDelimiter);
        var metadata = input.Substring(0, index < 0 ? 0 : index);
        var value = input.Substring(index + 1);

        return (metadata.Split(FieldKeyConstants.MetadataDelimiter), SplitAddress(value));
    }

    private string[] SplitAddress(string datum) => datum.Split(FieldKeyConstants.MetadataDelimiter);

    private static AddressType? ParseAddressType(string type)
    {
        AddressType? addressType = null;
        var typeGroups = type.Split(',');

        foreach (var typeGroup in typeGroups)
        {
            var parsedValue = typeGroup.ParseAddressType();

            if (parsedValue == null)
                continue;

            if (addressType.HasValue)
                addressType |= parsedValue.Value;
            else
                addressType = parsedValue.Value;
        }

        return addressType;
    }
}
