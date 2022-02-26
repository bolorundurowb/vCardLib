using System;

namespace vCardLib.Extensions;

internal static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string input, string value) =>
        input.Equals(value, StringComparison.CurrentCultureIgnoreCase);

    public static bool StartsWithIgnoreCase(this string input, string value) =>
        input.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
}