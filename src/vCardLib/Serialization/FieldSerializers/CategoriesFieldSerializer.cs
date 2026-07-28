using System.Collections.Generic;
using System.Linq;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class CategoriesFieldSerializer : IV2FieldSerializer<List<string>>, IV3FieldSerializer<List<string>>,
    IV4FieldSerializer<List<string>>
{
    public string FieldKey => "CATEGORIES";

    // v2.1 has no backslash escaping mechanism.
    string? IV2FieldSerializer<List<string>>.Write(List<string> data) => Format(data, escape: false);

    public string? Write(List<string> data) => Format(data, escape: true);

    private string Format(List<string> data, bool escape)
    {
        var items = escape ? data.Select(x => ValueEscaper.Escape(x)) : data;
        return $"{FieldKey}:{string.Join(",", items)}";
    }
}
