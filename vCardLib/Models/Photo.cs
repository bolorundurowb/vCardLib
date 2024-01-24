using System;

namespace vCardLib.Models;

public struct Photo
{
    /// <summary>
    /// The image data type
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

    public string? Value { get; set; }

    public string Data { get; set; }

    public Photo(string data, string? encoding = null, string? type = null, string? mimeType = null,
        string? value = null)
    {
        Data = data;
        Encoding = encoding;
        Type = type;
        MimeType = mimeType;
        Value = value;
    }

    public byte[] AsByteArray() => Convert.FromBase64String(Data);
}
