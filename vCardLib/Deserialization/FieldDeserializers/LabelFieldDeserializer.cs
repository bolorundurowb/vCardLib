using System.Text.RegularExpressions;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;
using vCardLib.Utilities;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class LabelFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Label>,
    IV3FieldDeserializer<Label>,
    IV4FieldDeserializer<Label?>
{
    public string FieldKey => "LABEL";

    public Label Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        if (metadata.Length == 0)
            return new Label(Regex.Unescape(value));

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

        return new Label(Regex.Unescape(value), type);
    }

    Label? IV4FieldDeserializer<Label?>.Read(string input) => null;
}
