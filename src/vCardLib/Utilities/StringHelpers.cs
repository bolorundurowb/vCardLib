namespace vCardLib.Utilities;

internal static class StringHelpers
{
    public static bool IsQuoted(string? input)
    {
        if (input is null)
            return false;

        var trimmedInput = input.Trim();
        return trimmedInput.Length >= 2
               && trimmedInput.StartsWith("\"", System.StringComparison.Ordinal)
               && trimmedInput.EndsWith("\"", System.StringComparison.Ordinal);
    }
}