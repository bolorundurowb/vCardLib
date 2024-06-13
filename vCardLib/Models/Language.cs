namespace vCardLib.Models;

/// <summary>
/// Represents a language in a vCard.
/// </summary>
public struct Language
{
    /// <summary>
    /// Gets or sets the locale code for the language (e.g., en-US, fr-FR).
    /// </summary>
    public string Locale { get; set; }

    /// <summary>
    /// Gets or sets the preference value for this language (e.g., higher number indicates stronger preference).
    /// </summary>
    public int? Preference { get; set; }

    /// <summary>
    /// Gets or sets the type of language (e.g., text, speech).
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Language"/> structure.
    /// </summary>
    /// <param name="locale">The locale code for the language.</param>
    /// <param name="preference">The preference value for this language (optional).</param>
    /// <param name="type">The type of language (optional).</param>
    public Language(string locale, int? preference = null, string? type = null)
    {
        Locale = locale;
        Preference = preference;
        Type = type;
    }
}