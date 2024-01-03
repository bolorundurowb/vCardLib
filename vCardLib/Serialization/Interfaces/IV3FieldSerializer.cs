namespace vCardLib.Serialization.Interfaces;

internal interface IV3FieldSerializer<in TModel> : IFieldSerializer
{
    string Write(TModel data);
}
