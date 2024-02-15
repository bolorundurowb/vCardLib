using System;
using System.Linq;
using System.Text;
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

    public string Write(Address data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != AddressType.None)
        {
            var addressTypes = Enum.GetValues(typeof(AddressType))
                .Cast<AddressType>()
                .Where(x => data.Type.HasFlag(x) && x != AddressType.None)
                .ToArray();

            if (addressTypes.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);

                foreach (var addressType in addressTypes)
                    builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, addressType.DecomposeAddressType());
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
