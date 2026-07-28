namespace vCardLib.Serialization.Utilities;

internal static class ValueEscaper
{
    /// <summary>
    /// Escapes a single structured or list component per RFC 2426 &amp; RFC 6350 3.4.
    /// Structural separators are added by the caller and must not be escaped here.
    /// </summary>
    public static string Escape(string? data)
    {
        if (string.IsNullOrEmpty(data)) return data ?? string.Empty;

        return data!
            .Replace(@"\", @"\\")   // Must be first
            .Replace(";", @"\;")
            .Replace(",", @"\,")
            .Replace("\r\n", @"\n") // Normalize Windows line endings
            .Replace("\n", @"\n")
            .Replace("\r", @"\n");  // Handle stray CRs
    }
}
