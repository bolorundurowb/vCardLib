using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vCardLib.Constants;

namespace vCardLib.Utilities;

/// <summary>
/// Class to hold all supporting methods
/// </summary>
public static class Helpers
{
    internal static IEnumerable<string[]> GetContactsFromFile(string filePath)
    {
        var stream = new FileStream(filePath, FileMode.Open);
        return GetContactsFromStream(stream);
    }

    internal static IEnumerable<string[]> GetContactsFromStream(Stream stream)
    {
        var encoding = GetEncoding(stream);
        using (var reader = new StreamReader(stream, encoding))
        {
            var contents = reader.ReadToEnd();
            return GetContactsFromString(contents);
        }
    }

    [Obsolete("Use GetContactsFromStream(stream) instead.")]
    internal static IEnumerable<string[]> GetContactsFromStreamReader(StreamReader streamReader)
    {
        var contents = streamReader.ReadToEnd();
        return GetContactsFromString(contents);
    }

    internal static IEnumerable<string[]> GetContactsFromString(string contents)
    {
        var contacts = GetIndividualContacts(contents);

        return contacts
            .Select(ExtractContactDetails)
            .Where(x => x.Length > 0);
    }

    private static IEnumerable<string> GetIndividualContacts(string allContacts)
    {
        if (string.IsNullOrWhiteSpace(allContacts))
        {
            return new string[0];
        }

        if (!allContacts.Contains(FieldKeyConstants.StartToken) || !allContacts.Contains(FieldKeyConstants.EndToken))
        {
            throw new InvalidOperationException("The vCard contents are invalid.");
        }

        return allContacts.Split(new[]
        {
            FieldKeyConstants.EndToken
        }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static string[] ExtractContactDetails(string contact)
    {
        var normalizedContact = contact
            .Replace("PREF;", "")
            .Replace("pref;", "")
            .Replace("PREF,", "")
            .Replace("pref,", "")
            .Replace(",PREF", "")
            .Replace(",pref", "");

        return normalizedContact.Split(new[]
        {
            "\n", "\r\n"
        }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static Encoding GetEncoding(Stream stream)
    {
        // Read the BOM
        var bom = new byte[4];
        stream.Read(bom, 0, 4);

        // reset the stream
        stream.Position = 0;

        // Analyze the BOM
        if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
        if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
        {
            return Encoding.UTF8;
        }

        if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode;
        if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode;
        if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;

        return Encoding.ASCII;
    }
}