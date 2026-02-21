using System.Linq;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class EmailAddressFieldDeserializer : IV2FieldDeserializer<EmailAddress>,
    IV3FieldDeserializer<EmailAddress>, IV4FieldDeserializer<EmailAddress>
{
    public static string FieldKey => "EMAIL";

    public EmailAddress Read(string input)
    {
        return ReadInternal(input, false);
    }

    EmailAddress IV4FieldDeserializer<EmailAddress>.Read(string input)
    {
        return ReadInternal(input, true);
    }

    private static EmailAddress ReadInternal(string input, bool numericPreference)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var parameters = VCardParameters.Parse(metadata);

        var type = ParameterInterpreters.ParseTypeFlags<EmailAddressType>(parameters, SharedParsers.ParseEmailAddressType, FieldKeyConstants.PreferenceKey);
        var preference = ParameterInterpreters.ParsePreference(parameters, numericPreference);

        return new EmailAddress(value, type, preference);
    }
}
