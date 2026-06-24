using System;
using System.Collections.Generic;
using System.Linq;
using vCardLib.Models;

namespace vCardLib.Serialization;

internal sealed class SerializerAdapter<T>(Func<vCard, T?> getter, Func<T, string?> writer) : IFieldSerializer
{
    public IEnumerable<string>? Serialize(vCard card)
    {
        var value = getter(card);
        if (value == null)
            return null;

        var line = writer(value);
        return line == null ? null : new[] { line };
    }
}

internal sealed class CollectionSerializerAdapter<T>(Func<vCard, IEnumerable<T>> getter, Func<T, string?> writer)
    : IFieldSerializer
{
    public IEnumerable<string>? Serialize(vCard card)
    {
        var items = getter(card);
        List<string>? lines = items?.Select(writer).Where(x => x != null).ToList()!;
        return lines?.Any() == true ? null : lines.AsEnumerable();
    }
}