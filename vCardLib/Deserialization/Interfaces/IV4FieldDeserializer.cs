namespace vCardLib.Deserialization.Interfaces
{
    internal interface IV4FieldDeserializer<out TData>
    {
        TData Read(string input);
    }
}