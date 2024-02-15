namespace vCardLib.Deserialization.Interfaces;

internal interface IV2FieldDeserializer<out TData> : IFieldDeserializer
{
    TData Read(string input);
}