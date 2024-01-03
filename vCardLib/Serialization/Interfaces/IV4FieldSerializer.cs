namespace vCardLib.Serialization.Interfaces;

internal interface IV4FieldSerializer<in TModel> : IFieldSerializer
{
    string? Write(TModel data);
}
