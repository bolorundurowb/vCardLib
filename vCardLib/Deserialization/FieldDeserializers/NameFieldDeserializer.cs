﻿using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class NameFieldDeserializer : IV2FieldDeserializer<Name>, IV3FieldDeserializer<Name>,
    IV4FieldDeserializer<Name>
{
    public static string FieldKey => "N";

    public Name Read(string input)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        var value = input.Substring(separatorIndex + 1).Trim();
        string? familyName = null,
            givenName = null,
            additionalNames = null,
            honorificPrefix = null,
            honorificSuffix = null;

        var parts = value.Split(FieldKeyConstants.MetadataDelimiter);
        var partsLength = parts.Length;

        if (partsLength > 0)
            familyName = parts[0];

        if (partsLength > 1)
            givenName = parts[1];

        if (partsLength > 2)
            additionalNames = parts[2];

        if (partsLength > 3)
            honorificPrefix = parts[3];

        if (partsLength > 4)
            honorificSuffix = parts[4];

        return new Name(familyName, givenName, additionalNames, honorificPrefix, honorificSuffix);
    }
}