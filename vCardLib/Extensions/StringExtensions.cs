using System;

namespace vCardLib.Extensions;

internal static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string input, string value) =>
        input.Equals(value, StringComparison.InvariantCultureIgnoreCase);

    public static bool StartsWithIgnoreCase(this string input, string value) =>
        input.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);

    public static string ReplaceIgnoreCase(this string input, string oldValue, string newValue) =>
        input.Replace(oldValue, newValue, StringComparison.InvariantCultureIgnoreCase);
}