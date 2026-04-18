using vCardLib.Enums;

namespace vCardLib.Models;

/// <summary>   
/// Represents a URL in a vCard
/// </summary>
public struct Url
{
    /// <summary>
    /// Gets or sets the URL type (e.g. Work, Home, etc.).
    /// </summary>
    public UrlType? Type { get; set; }

    /// <summary>
    /// Gets or sets the textual representation of the URL
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the preference level for this URL. Usually when there are multiple
    /// </summary>
    public int? Preference { get; set; }

    /// <summary>
    /// Gets or sets the human-readable label for this URL
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the resource linked by the URL
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets the language of the resource
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Specifies the character set used in the resource.
    /// </summary>
    public string? Charset { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Url"/> struct.
    /// </summary>
    /// <param name="value">The textual representation of the URL</param>
    /// <param name="type">The URL type (optional)</param>
    /// <param name="preference">The preference level for this URL (optional)</param>
    /// <param name="label">The human-readable label for this URL (optional)</param>
    /// <param name="mimeType">The MIME type of the resource linked by the URL (optional)</param>
    /// <param name="language">The language of the resource (optional)</param>
    /// <param name="charset">Specifies the character set used in the resource (optional)</param>
    public Url(string value, UrlType? type = null, int? preference = null, string? label = null,
        string? mimeType = null, string? language = null, string? charset = null)
    {
        Value = value;
        Type = type;
        Preference = preference;
        Label = label;
        MimeType = mimeType;
        Language = language;
        Charset = charset;
    }
}