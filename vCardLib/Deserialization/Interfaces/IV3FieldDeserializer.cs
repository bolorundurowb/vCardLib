namespace vCardLib.Deserialization.Interfaces;

internal interface IV3FieldDeserializer<out TData> : IFieldDeserializer
{
    TData Read(string input);
}