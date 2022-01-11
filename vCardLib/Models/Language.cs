namespace vCardLib.Models;

public struct Language
{
    public string Type { get; set; }

    public int? Preference { get; set; }

    public string? Locale { get; set; }

    public Language(string type, int? preference, string? locale)
    {
        Type = type;
        Preference = preference;
        Locale = locale;
    }
}