﻿using System;
using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class LabelFieldSerializer : IV2FieldSerializer<Label>, IV3FieldSerializer<Label>,
    IV4FieldSerializer<Label>
{
    public string FieldKey => "LABEL";

    public string? Write(Label data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != AddressType.None)
        {
            var addressTypes = Enum.GetValues(data.GetType())
                .Cast<AddressType>()
                .Where(x => data.Type.HasFlag(x))
                .ToArray();

            if (addressTypes.Any())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);

                foreach (var addressType in addressTypes)
                    builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, addressType.DecomposeAddressType());
            }
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Text);

        return builder.ToString();
    }

    string? IV4FieldSerializer<Label>.Write(Label data) => null;
}
