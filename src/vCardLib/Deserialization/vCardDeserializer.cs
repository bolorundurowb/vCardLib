using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public static IEnumerable<vCard> FromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        if (!File.Exists(filePath))
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

        vcardContents = PrepareForParse(vcardContents);

        if (!vcardContents.TrimStart().StartsWith(FieldKeyConstants.StartToken, StringComparison.Ordinal))
            throw new Exception($"A vCard must begin with '{FieldKeyConstants.StartToken}'.");

        // Lenient: allow trailing newlines (e.g. after normalizing CRLF to LF) or other trailing whitespace.
        if (!vcardContents.TrimEnd('\r', '\n', ' ', '\t').EndsWith(FieldKeyConstants.EndToken, StringComparison.Ordinal))
            throw new Exception($"A vCard must end with '{FieldKeyConstants.EndToken}'.");

        if (!vcardContents.Contains(FieldKeyConstants.VersionKey))
            throw new Exception($"A vCard must contain a '{FieldKeyConstants.VersionKey}'.");

        var cardGroups = SplitContent(vcardContents);

        foreach (var vcardContent in cardGroups)
            yield return Convert(vcardContent);
    }

    #region Private Helpers

    private static string PrepareForParse(string input)
    {
        if (input.Length > 0 && input[0] == '\uFEFF')
            input = input.Substring(1);

        return input.Replace("\r\n", "\n").Replace("\r", "\n");
    }

    private static IEnumerable<string[]> SplitContent(string vcardContent)
    {
        var unfoldedContent = Unfold(vcardContent);
        using var reader = new StringReader(unfoldedContent);
        var response = new List<string>();
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.EqualsIgnoreCase(FieldKeyConstants.EndToken))
            {
                yield return response.ToArray();
            }
            else if (line.EqualsIgnoreCase(FieldKeyConstants.StartToken))
            {
                response.Clear();
            }
            else
            {
                response.Add(line);
            }
        }
    }

    /// <summary>
    /// Unfolds RFC 2426/6350 folded lines (leading space or tab on a new physical line).
    /// Also joins vCard 2.1 lines that use a trailing '=' soft line break.
    /// </summary>
    private static string Unfold(string vcardContent)
    {
        if (string.IsNullOrEmpty(vcardContent)) return vcardContent;

        const char internalNewline = '\n';
        var builder = new StringBuilder();
        using var reader = new StringReader(vcardContent);
        string? line;
        string? previousLine = null;

        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(line)) continue;

            // RFC 2426/6350 Unfolding (Space/Tab)
            if (previousLine != null && (line[0] == ' ' || line[0] == '\t'))
            {
                previousLine += line.Substring(1);
            }
            // v2.1 Quoted-Printable continuation (Line ending with =)
            else if (previousLine != null && previousLine.EndsWith("=", StringComparison.Ordinal))
            {
                previousLine = previousLine.Substring(0, previousLine.Length - 1) + line;
            }
            else
            {
                if (previousLine != null)
                {
                    builder.Append(previousLine);
                    builder.Append(internalNewline);
                }
                previousLine = line;
            }
        }

        if (previousLine != null)
        {
            builder.Append(previousLine);
            builder.Append(internalNewline);
        }

        return builder.ToString();
    }

    private static vCard Convert(string[] vcardContent)
    {
        var versionRow = vcardContent.FirstOrDefault(x => x.StartsWith(VersionDeserializer.FieldKey, StringComparison.OrdinalIgnoreCase));

        if (versionRow == null)
            throw new ArgumentException("No version specified");

        var version = VersionDeserializer.Read(versionRow);

        Func<string, IFieldDeserializer?> registry = version switch
        {
            vCardVersion.v2 => V2DeserializerRegistry.Get,
            vCardVersion.v3 => V3DeserializerRegistry.Get,
            vCardVersion.v4 => V4DeserializerRegistry.Get,
            _ => throw new ArgumentException($"Unsupported version: {version}")
        };

        var card = new vCard(version);
        var seenKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var customDeserializer = new CustomFieldDeserializer();

        foreach (var line in vcardContent)
        {
            var key = ExtractKey(line);
            if (key == null)
                continue;

            // First PHOTO line also populates Logo
            if (key.Equals("PHOTO", StringComparison.OrdinalIgnoreCase) && !seenKeys.Contains("PHOTO"))
            {
                var photoDeserializer = registry("PHOTO");
                if (photoDeserializer != null)
                {
                    card.Logo = ReadPhoto(line, version);
                }
            }

            var deserializer = registry(key);
            if (deserializer != null)
            {
                deserializer.Deserialize(line, card);
            }
            else if (!IsKnownStructuralKey(key))
            {
                customDeserializer.Deserialize(line, card);
            }

            seenKeys.Add(key);
        }

        return card;
    }

    private static string? ExtractKey(string line)
    {
        if (line.EqualsIgnoreCase(FieldKeyConstants.StartToken) ||
            line.EqualsIgnoreCase(FieldKeyConstants.EndToken) ||
            line.StartsWith(VersionDeserializer.FieldKey, StringComparison.OrdinalIgnoreCase))
            return null;

        var colonIndex = line.IndexOf(FieldKeyConstants.SectionDelimiter);
        if (colonIndex < 0)
            return null;

        var beforeColon = line.Substring(0, colonIndex);

        var dotIndex = beforeColon.LastIndexOf('.');
        if (dotIndex >= 0)
            beforeColon = beforeColon.Substring(dotIndex + 1);

        var semicolonIndex = beforeColon.IndexOf(FieldKeyConstants.MetadataDelimiter);
        if (semicolonIndex >= 0)
            beforeColon = beforeColon.Substring(0, semicolonIndex);

        return beforeColon;
    }

    private static bool IsKnownStructuralKey(string key)
    {
        return key.Equals(FieldKeyConstants.StartToken, StringComparison.OrdinalIgnoreCase) ||
               key.Equals(FieldKeyConstants.EndToken, StringComparison.OrdinalIgnoreCase) ||
               key.Equals(VersionDeserializer.FieldKey, StringComparison.OrdinalIgnoreCase);
    }

    private static Photo ReadPhoto(string line, vCardVersion version)
    {
        return version switch
        {
            vCardVersion.v2 => ((IV2FieldDeserializer<Photo>)new PhotoFieldDeserializer()).Read(line),
            vCardVersion.v3 => ((IV3FieldDeserializer<Photo>)new PhotoFieldDeserializer()).Read(line),
            vCardVersion.v4 => ((IV4FieldDeserializer<Photo>)new PhotoFieldDeserializer()).Read(line),
            _ => throw new ArgumentException($"Unsupported version: {version}")
        };
    }

    #endregion
}
