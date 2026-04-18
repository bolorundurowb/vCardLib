using System.Collections.Generic;
using vCardLib.Constants;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class AddressFieldSerializer : IV2FieldSerializer<Address>, IV3FieldSerializer<Address>,
    IV4FieldSerializer<Address>
{
    public string FieldKey => "ADR";

    string IV2FieldSerializer<Address>.Write(Address data) => Write(data, vCardVersion.v2);
    string IV3FieldSerializer<Address>.Write(Address data) => Write(data, vCardVersion.v3);
    string IV4FieldSerializer<Address>.Write(Address data) => Write(data, vCardVersion.v4);

    public string Write(Address data) => Write(data, vCardVersion.v3);

    private string Write(Address data, vCardVersion version)
    {
        var types = data.Type.DecomposeAddressTypes();
        var extra = new List<(string Key, string Value)>();

        if (!string.IsNullOrWhiteSpace(data.Label))
        {
            extra.Add((LabelFieldDeserializer.FieldKey, data.Label!));
        }

        if (data.Geographic != null)
        {
            extra.Add((GeoFieldDeserializer.FieldKey, $"{data.Geographic.Value.Latitude},{data.Geographic.Value.Longitude}"));
        }

        var parameters = SerializationHelpers.FormatParameters(version, types, null, extra);

        var value = string.Join(FieldKeyConstants.MetadataDelimiter.ToString(),
            data.PostOfficeBox, data.ApartmentOrSuiteNumber, data.StreetAddress, data.CityOrLocality,
            data.StateOrProvinceOrRegion, data.PostalOrZipCode, data.Country);

        return $"{FieldKey}{parameters}{FieldKeyConstants.SectionDelimiter}{value}";
    }
}
