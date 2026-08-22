using System;
using System.Text;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Extensions;

namespace vCardLib.Deserialization.FieldDeserializers;

internal abstract class TextFieldDeserializer : IV2FieldDeserializer<string>, IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    string IV2FieldDeserializer<string>.Read(string input)
    {
        var (parameters, value) = DataSplitHelpers.SplitLine("DUMMY", input);
        var isQuotedPrintable = false;

        foreach (var (key, val) in DataSplitHelpers.ParseParameters(parameters))
        {
            if (key != null && key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey) && val.EqualsIgnoreCase("QUOTED-PRINTABLE"))
            {
                isQuotedPrintable = true;
            }
        }

        if (isQuotedPrintable) value = SharedParsers.DecodeQuotedPrintable(value);

        return value;
    }

    string IV3FieldDeserializer<string>.Read(string input) => Read(input);
    string IV4FieldDeserializer<string>.Read(string input) => Read(input);

    private string Read(string input)
    {
        var (parameters, value) = DataSplitHelpers.SplitLine("DUMMY", input);
        var isQuotedPrintable = false;

        foreach (var (key, val) in DataSplitHelpers.ParseParameters(parameters))
        {
            if (key != null && key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey) && val.EqualsIgnoreCase("QUOTED-PRINTABLE"))
            {
                isQuotedPrintable = true;
            }
        }

        if (isQuotedPrintable) value = SharedParsers.DecodeQuotedPrintable(value);

        return ValueUnescaper.Unescape(value, handleNewlines: true);
    }
}
