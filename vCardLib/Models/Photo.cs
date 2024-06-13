using System;

namespace vCardLib.Models;

/// <summary>
/// Represents a photo included in a vCard.
/// </summary>
public struct Photo
{
    /// <summary>
    /// Gets or sets the image data type (e.g., JPEG, PNG).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the image data (e.g., image/jpeg).
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets the encoding of the image data (e.g., BASE64).
    /// </summary>
    public string? Encoding { get; set; }

    /// <summary>
    /// Gets or sets a textual representation of the image data (likely for debugging purposes).
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets the image data as a byte array.
    /// </summary>
    /// <remarks>
    /// This method assumes the image data is encoded in Base64. If a different encoding is used, 
    /// you will need to use an appropriate decoding method before conversion to a byte array.
    /// </remarks>
    public string Data { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Photo"/> structure.
    /// </summary>
    /// <param name="data">The image data in Base64 encoding.</param>
    /// <param name="encoding">The encoding of the image data (optional).</param>
    /// <param name="type">The image data type (optional).</param>
    /// <param name="mimeType">The MIME type of the image data (optional).</param>
    /// <param name="value">A textual representation of the image data (optional).</param>
    public Photo(string data, string? encoding = null, string? type = null, string? mimeType = null,
        string? value = null)
    {
        Data = data;
        Encoding = encoding;
        Type = type;
        MimeType = mimeType;
        Value = value;
    }

    /// <summary>
    /// Converts the Base64 encoded image data to a byte array.
    /// </summary>
    /// <returns>A byte array representing the image data.</returns>
    public byte[] AsByteArray() => Convert.FromBase64String(Data);
}