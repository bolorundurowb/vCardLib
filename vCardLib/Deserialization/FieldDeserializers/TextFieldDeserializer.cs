using System;
using System.Text;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal abstract class TextFieldDeserializer : IV2FieldDeserializer<string>, IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    string IV4FieldDeserializer<string>.Read(string input) => ParseValue(input, handleNewlines: true);

    string IV3FieldDeserializer<string>.Read(string input) => ParseValue(input, handleNewlines: true);

    string IV2FieldDeserializer<string>.Read(string input)
    {
        // STRICT COMPLIANCE NOTE:
        // vCard 2.1 does NOT officially support backslash escaping for text (like \, or \n).
        // It uses Quoted-Printable for newlines.
        // However, many implementations incorrectly use v3-style escaping in v2.
        // If you want strict v2, simply return the substring. 
        // If you want "pragmatic" (tolerant) v2, use the parser below but maybe disable newlines.
        
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        // Usually 2.1 is just the raw value (or decoded from Quoted-Printable elsewhere)
        return input.Substring(separatorIndex + 1).Trim(); 
    }

    private static string ParseValue(string input, bool handleNewlines)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        if (separatorIndex < 0) 
            return string.Empty;

        var source = input.Substring(separatorIndex + 1).Trim();
        if (string.IsNullOrEmpty(source)) 
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