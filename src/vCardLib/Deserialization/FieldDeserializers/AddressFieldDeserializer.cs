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
        var (parameters, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        AddressType? type = null;
        Geo? geo = null;
        string? label = null;
        var isQuotedPrintable = false;

        foreach (var (key, val) in DataSplitHelpers.ParseParameters(parameters))
        {
            if (key == null || key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
            {
                var parsedType = val.ParseAddressType();
                if (parsedType.HasValue)
                    type = type.HasValue ? type.Value | parsedType : parsedType;
            }
            else if (key.EqualsIgnoreCase(GeoFieldDeserializer.FieldKey))
            {
                geo = (new GeoFieldDeserializer() as IV4FieldDeserializer<Geo>).Read(val);
            }
            else if (key.EqualsIgnoreCase(LabelFieldDeserializer.FieldKey))
            {
                label = val;
            }
            else if (key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey) && val.EqualsIgnoreCase("QUOTED-PRINTABLE"))
            {
                isQuotedPrintable = true;
            }
        }

        if (isQuotedPrintable) value = SharedParsers.DecodeQuotedPrintable(value);

        var values = value.Split(FieldKeyConstants.MetadataDelimiter);
        if (values.Length != 7)
            throw new Exception("Address parts incomplete");

        return new Address(values[0], values[1], values[2], values[3], values[4], values[5], values[6], type, label,
            geo);
    }
}
