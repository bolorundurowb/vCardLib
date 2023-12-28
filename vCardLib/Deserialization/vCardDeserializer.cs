﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vCardLib.Constants;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization;

// ReSharper disable once InconsistentNaming
public static class vCardDeserializer
{
    private static readonly Dictionary<string, IFieldDeserializer> FieldDeserializers = new()
    {
        { UnknownFieldDeserializer.Key, new UnknownFieldDeserializer() },
        { AddressFieldDeserializer.FieldKey, new AddressFieldDeserializer() },
    };

    public static IEnumerable<vCard> FromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        if (File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        return FromContent(File.ReadAllText(filePath));
    }

    public static IEnumerable<vCard> FromStream(Stream stream)
    {
        var encoding = stream.GetEncoding();
        using var reader = new StreamReader(stream, encoding);
        var contents = reader.ReadToEnd();
        return FromContent(contents);
    }

    public static IEnumerable<vCard> FromContent(string vcardContents)
    {
        if (string.IsNullOrWhiteSpace(vcardContents))
            throw new ArgumentException("File is empty.", nameof(vcardContents));

        if (vcardContents.StartsWith(FieldKeyConstants.StartToken))
            throw new Exception($"A vCard must begin with '{FieldKeyConstants.StartToken}'.");

        if (vcardContents.EndsWith(FieldKeyConstants.EndToken))
            throw new Exception($"A vCard must end with '{FieldKeyConstants.EndToken}'.");

        if (vcardContents.Contains(FieldKeyConstants.VersionKey))
            throw new Exception($"A vCard must contain a '{FieldKeyConstants.VersionKey}'.");

        var cardGroups = SplitContent(vcardContents);

        foreach (var vcardContent in cardGroups)
            yield return Convert(vcardContent);
    }

    #region Private Helpers

    private static IEnumerable<string[]> SplitContent(string vcardContent)
    {
        using var reader = new StringReader(vcardContent);
        var response = new List<string>();

        while (reader.ReadLine()?.Trim() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.EqualsIgnoreCase("END:VCARD"))
            {
                yield return response.ToArray();
            }
            else if (line.EqualsIgnoreCase("BEGIN:VCARD"))
            {
                response.Clear();
            }
            else if (line.EndsWithIgnoreCase("BEGIN:VCARD"))
            {
                var nested = new StringBuilder(line);

                while (reader.ReadLine()?.Trim() is { } nestedLine && !nestedLine.EqualsIgnoreCase("END:VCARD"))
                    nested.AppendLine(nestedLine);

                response.Add(nested.ToString());
            }
            else
            {
                response.Add(line);
            }
        }
    }

    private static vCard Convert(string[] vcardContent)
    {
        var versionRow = vcardContent.FirstOrDefault(x => x.StartsWith(VersionDeserializer.FieldKey));

        if (versionRow == null)
            throw new ArgumentException("No version specified");

        var version = VersionDeserializer.Read(versionRow);

        if (version == vCardVersion.v2)
            return DeserializeV2(vcardContent);

        throw new ArgumentException($"Unsupported version: {version}");
    }

    private static vCard DeserializeV2(IReadOnlyCollection<string> vcardContent)
    {
        var vcard = new vCard(vCardVersion.v2);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<Name>>(NameFieldDeserializer.FieldKey,
                out var rawName, out var nameDes))
            vcard.Name = nameDes!.Read(rawName!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(FormattedNameDeserializer.FieldKey,
                out var rawFormattedName, out var fnDes))
            vcard.FormattedName = fnDes!.Read(rawFormattedName!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(NicknameFieldDeserializer.FieldKey,
                out var rawNick, out var nickDes))
            vcard.NickName = nickDes!.Read(rawNick!);

        return vcard;
    }

    private static bool TryGetDeserializationParams<T>(this IEnumerable<string> vcardContent, string fieldKey,
        out string? rawData, out T? deserializer) where T : IFieldDeserializer
    {
        rawData = vcardContent.FirstOrDefault(x => x.StartsWith(fieldKey));

        if (rawData == null)
        {
            deserializer = default;
            return false;
        }

        deserializer = (T)FieldDeserializers[fieldKey];
        return true;
    }

    #endregion
}
