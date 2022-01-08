namespace vCardLib.Deserialization.Interfaces;

internal interface IV3FieldDeserializer<out TData>
{
    TData Read(string input);
}