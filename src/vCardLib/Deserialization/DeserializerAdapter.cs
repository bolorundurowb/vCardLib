using System;
using vCardLib.Models;

namespace vCardLib.Deserialization;

internal sealed class DeserializerAdapter<T>(string fieldKey, Func<string, T> read, Action<vCard, T> apply)
    : IFieldDeserializer
{
    public string FieldKey => fieldKey;
    public void Deserialize(string line, vCard card) => apply(card, read(line));
}
