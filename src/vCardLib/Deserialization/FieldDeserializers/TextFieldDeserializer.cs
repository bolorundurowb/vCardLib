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

        return ParseValue(value, handleNewlines: true);
    }

    private static string ParseValue(string source, bool handleNewlines)
    {
        if (string.IsNullOrEmpty(source) || !source.Contains("\\"))
            return source;

        var sb = new StringBuilder(source.Length);

        for (var i = 0; i < source.Length; i++)
        {
            var c = source[i];

            // Check for Escape Character
            if (c == '\\' && i + 1 < source.Length)
            {
                var next = source[i + 1];

                // Handle Newlines (\n or \N)
                if (handleNewlines && (next == 'n' || next == 'N'))
                {
                    sb.Append(Environment.NewLine);
                    i++; // Skip the 'n'
                }
                // Handle Escaped Characters (\\, \, \;)
                else if (next == '\\' || next == ',' || next == ';')
                {
                    sb.Append(next);
                    i++; // Skip the escaped char
                }
                // Handle "Invalid" Escapes (e.g. \r or \z)
                // RFC says: preserve the backslash if the escape is undefined.
                else
                {
                    sb.Append(c);
                }
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}
