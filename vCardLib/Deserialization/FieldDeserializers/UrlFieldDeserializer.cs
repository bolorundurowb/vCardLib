﻿using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;
using vCardLib.Utilities;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class UrlFieldDeserializer : IV2FieldDeserializer<Url>, IV3FieldDeserializer<Url>,
    IV4FieldDeserializer<Url>
{
    public static string FieldKey => "URL";

    public Url Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        if (metadata.Length == 0)
            return new Url(value);

        UrlType? type = null;
        int? pref = null;
        string? label = null, mimeType = null, language = null, charset = null;

        foreach (var metadatum in metadata)
        {
            var (key, data) = DataSplitHelpers.ExtractKeyValue(metadatum, '=');

            if (key is null || key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
            {
                var parsedType = EnumExtensions.Parse<UrlType>(data);
                type = type is null ? parsedType : type | parsedType;
            }
            else if (key.EqualsIgnoreCase(FieldKeyConstants.LabelKey))
                label = StringHelpers.IsQuoted(data) ? data.Trim().Trim('"') : data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.CharacterSetKey))
                charset = data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.LanguageSetKey))
                language = data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.PreferenceKey))
                pref = int.Parse(data);
            else if (key.EqualsIgnoreCase(FieldKeyConstants.MediaTypeKey) ||
                     key.EqualsIgnoreCase(FieldKeyConstants.MediaTypeAltKey))
                mimeType = data;
        }

        return new Url(value, type, pref, label, mimeType, language, charset);
    }
}