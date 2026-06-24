using vCardLib.Models;

namespace vCardLib.Deserialization;

internal interface IFieldDeserializer
{
    string FieldKey { get; }
    void Deserialize(string line, vCard card);
}
