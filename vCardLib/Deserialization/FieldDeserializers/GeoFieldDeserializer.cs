﻿using System.Collections.Generic;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class GeoFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Geo>, IV3FieldDeserializer<Geo>,
    IV4FieldDeserializer<Geo>
{
    public string FieldKey => "GEO";

    public Geo Read(string input)
    {
        var parts = Sanitize(input).Split(';');
        return GenerateGeo(parts);
    }

    Geo IV4FieldDeserializer<Geo>.Read(string input)
    {
        var parts = Sanitize(input).Split(',');
        return GenerateGeo(parts);
    }

    private static string Sanitize(string input)
    {
        var index = input.LastIndexOf(':');
        return input.Substring(index + 1).Trim();
    }

    private static Geo GenerateGeo(IReadOnlyList<string> parts)
    {
        var latitude = float.Parse(parts[0]);
        var longitude = float.Parse(parts[1]);
        return new Geo(latitude, longitude);
    }
}