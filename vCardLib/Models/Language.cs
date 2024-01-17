namespace vCardLib.Models;

public struct Language
{
    public string Locale { get; set; }

    public int? Preference { get; set; }

    public string? Type { get; set; }

    public Language(string locale, int? preference = null, string? type = null)
    {
        Locale = locale;
        Preference = preference;
        Type = type;
    }
}
