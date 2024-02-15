namespace vCardLib.Serialization.Interfaces;

internal interface IV2FieldSerializer<in TModel> : IFieldSerializer
{
    string? Write(TModel data);
}
