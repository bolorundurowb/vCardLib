using System;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class AddressFieldDeserializer : IV2FieldDeserializer<Address>,
    IV3FieldDeserializer<Address>, IV4FieldDeserializer<Address>
{
    public static string FieldKey => "ADR";

    public Address Read(string input)
    {
        var (metadata, values) = SplitLine(input);

        if (values.Length != 7)
            throw new Exception("Address parts incomplete");

        var parameters = VCardParameters.Parse(metadata);

        var type = ParameterInterpreters.ParseTypeFlags<AddressType>(parameters, SharedParsers.ParseAddressType, GeoFieldDeserializer.FieldKey, LabelFieldDeserializer.FieldKey);
        var label = ParameterInterpreters.ParseStringParameter(parameters, LabelFieldDeserializer.FieldKey);
        
        Geo? geo = null;
        var geoStr = parameters.GetFirst(GeoFieldDeserializer.FieldKey);
        if (geoStr != null)
        {
            geo = (new GeoFieldDeserializer() as IV4FieldDeserializer<Geo>).Read(geoStr);
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
}
