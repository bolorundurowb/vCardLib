using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class KindFieldDeserializer :  IV2FieldDeserializer<ContactKind?>,
    IV3FieldDeserializer<ContactKind?>, IV4FieldDeserializer<ContactKind>
{
    public static string FieldKey => "KIND";

    public ContactKind? Read(string input) => null;

    ContactKind IV4FieldDeserializer<ContactKind>.Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        var value = input.Substring(separatorIndex + 1).Trim().ToLowerInvariant();

        return value switch
        {
            "group" => ContactKind.Group,
            "org" => ContactKind.Organization,
            "location" => ContactKind.Location,
            // NOTE: the RFC-6350 spec dictates that we default to individual if the value is un-parsable
            // https://datatracker.ietf.org/doc/html/rfc6350#section-6.1.4
            _ => ContactKind.Individual
        };
    }
}