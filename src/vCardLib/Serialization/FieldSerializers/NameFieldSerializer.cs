using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class NameFieldSerializer : IV2FieldSerializer<Name>, IV3FieldSerializer<Name>, IV4FieldSerializer<Name>
{
    public string FieldKey => "N";

    // v2.1 has no backslash escaping mechanism.
    string IV2FieldSerializer<Name>.Write(Name data) => Format(data, escape: false);

    public string Write(Name data) => Format(data, escape: true);

    private string Format(Name data, bool escape)
    {
        string E(string? component) => escape ? ValueEscaper.Escape(component) : component ?? string.Empty;
        var delimiter = FieldKeyConstants.MetadataDelimiter;

        return $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{E(data.FamilyName)}{delimiter}{E(data.GivenName)}{delimiter}{E(data.AdditionalNames)}{delimiter}{E(data.HonorificPrefix)}{delimiter}{E(data.HonorificSuffix)}";
    }
}
