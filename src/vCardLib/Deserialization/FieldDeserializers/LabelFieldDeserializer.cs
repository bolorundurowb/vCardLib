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

    // v2.1 has no backslash escaping, so keep the text verbatim.
    Label IV2FieldDeserializer<Label>.Read(string input) => Parse(input, unescape: false);

    public Label Read(string input) => Parse(input, unescape: true);

    Label? IV4FieldDeserializer<Label?>.Read(string input) => null;

    private static Label Parse(string input, bool unescape)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var text = unescape ? ValueUnescaper.Unescape(value) : value;

        if (metadata.Length == 0)
            return new Label(text);

        AddressType? type = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
            {
                if (string.IsNullOrWhiteSpace(data))
                    continue;

                var typeGroup = data!.Split(FieldKeyConstants.ConcatenationDelimiter);

                foreach (var individualType in typeGroup)
                {
                    var adrType = individualType.ParseAddressType();

                    if (adrType.HasValue)
                        type = type.HasValue ? type.Value | adrType : adrType;
                }
            }
        }

        return new Label(text, type);
    }
}
