using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class NameFieldSerializer : IV2FieldSerializer<Name>, IV3FieldSerializer<Name>, IV4FieldSerializer<Name>
{
    public string FieldKey => "N";

    public string Write(Name data) =>
        $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data.FamilyName}{FieldKeyConstants.MetadataDelimiter}{data.GivenName}{FieldKeyConstants.MetadataDelimiter}{data.AdditionalNames}{FieldKeyConstants.MetadataDelimiter}{data.HonorificPrefix}{FieldKeyConstants.MetadataDelimiter}{data.HonorificSuffix}";
}
