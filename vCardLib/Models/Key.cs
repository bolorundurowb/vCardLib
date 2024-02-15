namespace vCardLib.Models;

public struct Key
{
    public string? Type { get; set; }

    public string? MimeType { get; set; }

    public string? Encoding { get; set; }

    public string Value { get; set; }

    public Key(string value, string? type = null, string? mimeType = null, string? encoding = null)
    {
        Value = value;
        MimeType = mimeType != null ? mimeType.ToLowerInvariant() : null;
        Encoding = encoding != null ? encoding.ToLowerInvariant() : null;
        Type = type != null ? type.ToLowerInvariant() : null;
    }
}