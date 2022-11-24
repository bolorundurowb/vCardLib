using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class PhotoFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Photo>, IV3FieldDeserializer<Photo>, IV4FieldDeserializer<Photo>
{
    public string FieldKey => "PHOTO";

    Photo IV2FieldDeserializer<Photo>.Read(string input) => throw new System.NotImplementedException();
    
    Photo IV3FieldDeserializer<Photo>.Read(string input) => throw new System.NotImplementedException();
    
    Photo IV4FieldDeserializer<Photo>.Read(string input) => throw new System.NotImplementedException();
}
