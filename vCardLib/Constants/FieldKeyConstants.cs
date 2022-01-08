namespace vCardLib.Constants;

internal static class FieldKeyConstants
{
    public static char[] SectionDelimiter = { ':' };

    public static char[] KeyValueDelimiter = { '=' };

    public static char[] MetadataDelimiter = { ';' };

    public const string StartToken = "BEGIN:VCARD";

    public const string EndToken = "END:VCARD";

    public const string TypeKey = "TYPE";

    public const string PreferenceKey = "PREF";

    public const string EmailKey = "EMAIL";

    public const string TelKey = "TEL";
}