using System.Globalization;
using System.Text;
using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class GeoFieldSerializer : IV2FieldSerializer<Geo>, IV3FieldSerializer<Geo>, IV4FieldSerializer<Geo>
{
    public string FieldKey => "GEO";

    string? IV2FieldSerializer<Geo>.Write(Geo data)
    {
        var builder = new StringBuilder(FieldKey);
        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Latitude.ToString(CultureInfo.InvariantCulture));
        builder.Append(FieldKeyConstants.MetadataDelimiter);
        builder.Append(data.Longitude.ToString(CultureInfo.InvariantCulture));
        return builder.ToString();
    }

    string? IV3FieldSerializer<Geo>.Write(Geo data)
    {
        var builder = new StringBuilder(FieldKey);
        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Latitude.ToString(CultureInfo.InvariantCulture));
        builder.Append(FieldKeyConstants.MetadataDelimiter);
        builder.Append(data.Longitude.ToString(CultureInfo.InvariantCulture));
        return builder.ToString();
    }

    string? IV4FieldSerializer<Geo>.Write(Geo data)
    {
        var builder = new StringBuilder(FieldKey);
        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append("geo:");
        builder.Append(data.Latitude.ToString(CultureInfo.InvariantCulture));
        builder.Append(FieldKeyConstants.ConcatenationDelimiter);
        builder.Append(data.Longitude.ToString(CultureInfo.InvariantCulture));
        return builder.ToString();
    }
}
