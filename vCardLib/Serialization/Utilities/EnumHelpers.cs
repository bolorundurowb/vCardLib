namespace vCardLib.Serialization.Utilities;

internal static class EnumExtensions
{
    public static T Parse<T>(string value) where T : struct, System.Enum => (T)System.Enum.Parse(typeof(T), value);
}