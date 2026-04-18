using System.Collections.Generic;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class CategoriesFieldSerializer : IV2FieldSerializer<List<string>>, IV3FieldSerializer<List<string>>,
    IV4FieldSerializer<List<string>>
{
    public string FieldKey => "CATEGORIES";

    public string? Write(List<string> data)
    {
        var value = string.Join(",", data);
        return $"{FieldKey}: {value}";
    }
}
