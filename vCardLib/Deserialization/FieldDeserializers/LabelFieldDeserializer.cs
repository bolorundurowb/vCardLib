using System.Text.RegularExpressions;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class LabelFieldDeserializer : IV2FieldDeserializer<Label>, IV3FieldDeserializer<Label>,
    IV4FieldDeserializer<Label?>
{
    public static string FieldKey => "LABEL";

    public Label Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var parameters = VCardParameters.Parse(metadata);

        var type = ParameterInterpreters.ParseTypeFlags<AddressType>(parameters, SharedParsers.ParseAddressType);

        return new Label(Regex.Unescape(value), type);
    }

    Label? IV4FieldDeserializer<Label?>.Read(string input) => null;
}
