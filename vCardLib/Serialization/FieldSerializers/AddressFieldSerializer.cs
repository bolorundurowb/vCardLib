using System;
using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

using vCardLib.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class AddressFieldSerializer : IV2FieldSerializer<Address>, IV3FieldSerializer<Address>,
    IV4FieldSerializer<Address>
{
    public string FieldKey => "ADR";

    public string Write(Address data)
    {
        return WriteInternal(data, false);
    }

    string? IV4FieldSerializer<Address>.Write(Address data)
    {
        return WriteInternal(data, true);
    }

    private string WriteInternal(Address data, bool v4Style)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != AddressType.None)
        {
            var addressTypes = EnumCache<AddressType>.Values
                .Where(x => data.Type.HasFlag(x) && x != AddressType.None)
                .ToArray();

            if (addressTypes.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);

                if (v4Style)
                {
                    builder.AppendFormat("{0}=", FieldKeyConstants.TypeKey);
                    var typesList = addressTypes.Select(t => t.DecomposeAddressType()).ToList();
                    builder.Append(string.Join(FieldKeyConstants.ConcatenationDelimiter.ToString(), typesList));
                }
                else
                {
                    for (var i = 0; i < addressTypes.Length; i++)
                    {
                        if (i > 0)
                            builder.Append(FieldKeyConstants.MetadataDelimiter);

                        builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, addressTypes[i].DecomposeAddressType());
                    }
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(data.Label))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", LabelFieldDeserializer.FieldKey, data.Label);
        }

        if (data.Geographic != null)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1},{2}", GeoFieldDeserializer.FieldKey, data.Geographic.Value.Latitude,
                data.Geographic.Value.Longitude);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(string.Join(FieldKeyConstants.MetadataDelimiter.ToString(),
            data.PostOfficeBox, data.ApartmentOrSuiteNumber, data.StreetAddress, data.CityOrLocality,
            data.StateOrProvinceOrRegion, data.PostalOrZipCode, data.Country));

        return builder.ToString();
    }
}
