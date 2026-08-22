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

    // v2.1 has no backslash escaping, so split on the raw delimiter.
    Address IV2FieldDeserializer<Address>.Read(string input) => Parse(input, unescape: false);

    public Address Read(string input) => Parse(input, unescape: true);

    private static Address Parse(string input, bool unescape)
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

        var values = unescape
            ? ValueUnescaper.SplitUnescaped(value, FieldKeyConstants.MetadataDelimiter)
            : value.Split(FieldKeyConstants.MetadataDelimiter);
        if (values.Length != 7)
            throw new Exception("Address parts incomplete");

        string V(int index) => unescape ? ValueUnescaper.Unescape(values[index]) : values[index];

        return new Address(V(0), V(1), V(2), V(3), V(4), V(5), V(6), type, label, geo);
    }
}
