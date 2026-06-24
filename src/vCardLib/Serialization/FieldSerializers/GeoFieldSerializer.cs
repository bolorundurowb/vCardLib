using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class GeoFieldSerializer : IV2FieldSerializer<Geo>, IV3FieldSerializer<Geo>, IV4FieldSerializer<Geo>
{
    public string FieldKey => "GEO";

    public string? Write(Geo data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data.Latitude}{FieldKeyConstants.MetadataDelimiter}{data.Longitude}";
}
