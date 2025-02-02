namespace vCardLib.Utilities;

internal static class StringHelpers
{
    public static bool IsQuoted(string input)
    {
        var trimmedInput = input.Trim();
        return trimmedInput.StartsWith("\"") && trimmedInput.EndsWith("\"");
    }
}