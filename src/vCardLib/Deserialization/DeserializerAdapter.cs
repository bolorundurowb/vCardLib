using System;
using vCardLib.Models;

namespace vCardLib.Deserialization;

internal sealed class DeserializerAdapter<T> : IFieldDeserializer
{
    private readonly string _fieldKey;
    private readonly Func<string, T> _read;
    private readonly Action<vCard, T> _apply;

    public DeserializerAdapter(string fieldKey, Func<string, T> read, Action<vCard, T> apply)
    {
        _fieldKey = fieldKey;
        _read = read;
        _apply = apply;
    }

    public string FieldKey => _fieldKey;
    public void Deserialize(string line, vCard card) => _apply(card, _read(line));
}
