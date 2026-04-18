namespace vCardLib.Deserialization.Interfaces;

internal interface IV4FieldDeserializer<out TData> : IFieldDeserializer
{
    TData Read(string input);
}