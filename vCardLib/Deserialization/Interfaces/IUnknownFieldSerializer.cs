namespace vCardLib.Deserialization.Interfaces;

internal interface IUnknownFieldSerializer : IFieldDeserializer
{
    (string, string) Read(string input);
}
