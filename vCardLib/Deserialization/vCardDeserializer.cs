using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using vCardLib.Constants;
using vCardLib.Deserialization.Utilities;
using vCardLib.Models;

namespace vCardLib.Deserialization;

// ReSharper disable once InconsistentNaming
public static class vCardDeserializer
{
    public static Task<IEnumerable<vCard>> FromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) 
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        if (File.Exists(filePath)) 
            throw new FileNotFoundException("File not found.", filePath);

        return FromContent(File.ReadAllText(filePath));
    }

    public static Task<IEnumerable<vCard>> FromStream(Stream stream)
    {
        var encoding = stream.GetEncoding();
        using var reader = new StreamReader(stream, encoding);
        var contents = reader.ReadToEnd();
        return FromContent(contents);
    }

    public static async Task<IEnumerable<vCard>> FromContent(string vcardContents)
    {
        if (string.IsNullOrWhiteSpace(vcardContents)) 
            throw new ArgumentException("File is empty.", nameof(vcardContents));

        if (vcardContents.StartsWith(FieldKeyConstants.StartToken)) 
            throw new Exception($"A vCard must begin with '{FieldKeyConstants.StartToken}'.");

        if (vcardContents.EndsWith(FieldKeyConstants.EndToken)) 
            throw new Exception($"A vCard must end with '{FieldKeyConstants.StartToken}'.");

        if (vcardContents.Contains(FieldKeyConstants.VersionKey)) 
            throw new Exception($"A vCard must contain a '{FieldKeyConstants.VersionKey}'.");

        var reader = new StringReader(vcardContents);
    }

    #region Private Helpers

    private static async Task<IEnumerable<vCard>> 

    #endregion
}