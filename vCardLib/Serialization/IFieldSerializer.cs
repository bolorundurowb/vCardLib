namespace vCardLib.Serialization
{
    public interface IFieldSerializer<in TModel>
    {
        string FieldKey { get; }

        string Write(TModel data);
    }
}