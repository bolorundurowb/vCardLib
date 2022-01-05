namespace vCardLib.Deserialization.Interfaces
{
    internal interface IFieldDeserializer<out TModel>
    {
        string FieldKey { get; }

        TModel Read(string input);
    }
}