using System;

namespace vCardLib.Models;

public struct Photo
{
    /// <summary>
    /// The image data
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// The image type
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// The encoding of the image
    /// </summary>
    public string? Encoding { get; set; }

    public string Value { get; set; }

    public Photo(string value, string? encoding = null, string? type = null, string? mimeType = null)
    {
        Value = value;
        Encoding = encoding;
        Type = type;
        MimeType = mimeType;
    }

    public byte[] AsByteArray() => Convert.FromBase64String(Value);
}
