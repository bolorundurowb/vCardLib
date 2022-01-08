using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class GeoFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Geo>, IV3FieldDeserializer<Geo>,
    IV4FieldDeserializer<Geo>
{
    public string FieldKey => "GEO";

    public Geo Read(string input)
    {
        var index = input.LastIndexOf(':');
        var values = input.Substring(index + 1).Trim();
        var parts = values.Split(',');
        var latitude = float.Parse(parts[0]);
        var longitude = float.Parse(parts[1]);
        return new Geo(latitude, longitude);
    }
}