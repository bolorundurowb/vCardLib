using System;
using System.Runtime.CompilerServices;

namespace vCardLib.Extensions;

internal static class StringExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsIgnoreCase(this string input, string value) =>
        input.Equals(value, StringComparison.CurrentCultureIgnoreCase);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool StartsWithIgnoreCase(this string input, string value) =>
        input.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
}