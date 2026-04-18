namespace vCardLib.Models;

/// <summary>
/// Represents a key in a vCard.
/// </summary>
public struct Key
{
    /// <summary>
    /// Gets or sets the type of the key (e.g., WORK, HOME).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the key data (e.g., image/jpeg).
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets the encoding of the key data (e.g., BASE64).
    /// </summary>
    public string? Encoding { get; set; }

    /// <summary>
    /// Gets or sets the value of the key.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Key"/> structure.
    /// </summary>
    /// <param name="value">The value of the key.</param>
    /// <param name="type">The type of the key (optional).</param>
    /// <param name="mimeType">The MIME type of the key data (optional).</param>
    /// <param name="encoding">The encoding of the key data (optional).</param>
    public Key(string value, string? type = null, string? mimeType = null, string? encoding = null)
    {
        Value = value;
        MimeType = mimeType?.ToLowerInvariant();
        Encoding = encoding?.ToLowerInvariant();
        Type = type?.ToLowerInvariant();
    }
}