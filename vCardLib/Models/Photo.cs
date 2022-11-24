using System;
using vCardLib.Enums;

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

    public byte[] AsByteArray() => Convert.FromBase64String(Value);
}
