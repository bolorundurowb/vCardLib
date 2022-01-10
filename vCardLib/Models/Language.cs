namespace vCardLib.Models;

public struct Language
{
    public string Type { get; set; }
    
    public int? Preference { get; set; }
    
    public string Locale { get; set; }
}