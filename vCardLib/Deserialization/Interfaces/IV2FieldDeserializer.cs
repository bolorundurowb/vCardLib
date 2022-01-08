namespace vCardLib.Deserialization.Interfaces;

internal interface IV2FieldDeserializer<out TData>
{
    TData Read(string input);
}