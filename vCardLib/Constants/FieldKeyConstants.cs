namespace vCardLib.Constants;

internal static class FieldKeyConstants
{
    public const char SectionDelimiter = ':';

    public const char KeyValueDelimiter = '=';

    public const char MetadataDelimiter = ';';

    public const char ConcatenationDelimiter = ',';

    public const string StartToken = "BEGIN:VCARD";

    public const string EndToken = "END:VCARD";

    public const string TypeKey = "TYPE";

    public const string MediaTypeKey = "MEDIATYPE";

    public const string EncodingKey = "ENCODING";

    public const string ValueKey = "VALUE";

    public const string VersionKey = "VERSION";

    public const string PreferenceKey = "PREF";
}
