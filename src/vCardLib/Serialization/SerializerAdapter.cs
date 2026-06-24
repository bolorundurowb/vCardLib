using System;
using System.Collections.Generic;
using System.Linq;
using vCardLib.Models;

namespace vCardLib.Serialization;

internal sealed class SerializerAdapter<T> : IFieldSerializer
{
    private readonly Func<vCard, T?> _getter;
    private readonly Func<T, string?> _writer;

    public SerializerAdapter(Func<vCard, T?> getter, Func<T, string?> writer)
    {
        _getter = getter;
        _writer = writer;
    }

    public IEnumerable<string>? Serialize(vCard card)
    {
        var value = _getter(card);
        if (value == null) return null;
        var line = _writer(value);
        return line == null ? null : new[] { line };
    }
}

internal sealed class CollectionSerializerAdapter<T> : IFieldSerializer
{
    private readonly Func<vCard, IEnumerable<T>> _getter;
    private readonly Func<T, string?> _writer;

    public CollectionSerializerAdapter(Func<vCard, IEnumerable<T>> getter, Func<T, string?> writer)
    {
        _getter = getter;
        _writer = writer;
    }

    public IEnumerable<string>? Serialize(vCard card)
    {
        var items = _getter(card);
        var lines = items.Select(_writer).Where(x => x != null).ToList();
        return lines.Count == 0 ? null : lines;
    }
}
