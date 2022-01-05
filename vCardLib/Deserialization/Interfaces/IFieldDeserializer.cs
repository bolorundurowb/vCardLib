namespace vCardLib.Deserialization.Interfaces
{
    internal interface IFieldDeserializer<TModel>
    {
        string FieldKey { get; }

        TModel Read();
    }
}