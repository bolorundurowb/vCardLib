using System;
using System.Linq;
using vCardLib.Constants;

namespace vCardLib.Deserialization.Utilities;

/// <summary>
///     Provides helper methods for interpreting common vCard parameters.
/// </summary>
internal static class ParameterInterpreters
{
    /// <summary>
    ///     Parses the preference parameter.
    /// </summary>
    /// <param name="parameters">The parsed parameters.</param>
    /// <param name="numericOnly">True for v4 (numeric preference), false for v2/v3 (boolean existence).</param>
    /// <returns>The preference value, or null.</returns>
    public static int? ParsePreference(VCardParameters parameters, bool numericOnly)
    {
        if (numericOnly)
        {
            var prefStr = parameters.GetFirst(FieldKeyConstants.PreferenceKey);
            if (int.TryParse(prefStr, out var pref))
            {
                return pref;
            }
            return null;
        }

        return parameters.ContainsKey(FieldKeyConstants.PreferenceKey) ? 1 : null;
    }

    /// <summary>
    ///     Parses type flags from the parameters.
    /// </summary>
    /// <typeparam name="TEnum">The flags enum type.</typeparam>
    /// <param name="parameters">The parsed parameters.</param>
    /// <param name="parser">A function to parse a single string value into the enum.</param>
    /// <param name="ignoredKeys">Keys that should be ignored when looking for bare tokens.</param>
    /// <returns>The aggregated flags enum, or null if none found.</returns>
    public static TEnum? ParseTypeFlags<TEnum>(VCardParameters parameters, Func<string, TEnum?> parser, params string[] ignoredKeys) where TEnum : struct, Enum
    {
        TEnum result = default;
        bool found = false;

        // Handle TYPE=value
        var typeValues = parameters.GetAll(FieldKeyConstants.TypeKey);
        foreach (var value in typeValues)
        {
            if (string.IsNullOrWhiteSpace(value)) continue;

            var parts = value.Split(FieldKeyConstants.ConcatenationDelimiter);
            foreach (var part in parts)
            {
                var parsed = parser(part.Trim());
                if (parsed.HasValue)
                {
                    result = CombineEnum(result, parsed.Value);
                    found = true;
                }
            }
        }

        // Handle bare tokens (v2 style)
        foreach (var key in parameters.Keys)
        {
            if (ignoredKeys.Contains(key, StringComparer.OrdinalIgnoreCase)) continue;

            // If it's a bare token, GetFirst(key) == key
            if (string.Equals(key, parameters.GetFirst(key), StringComparison.OrdinalIgnoreCase))
            {
                var parsed = parser(key);
                if (parsed.HasValue)
                {
                    result = CombineEnum(result, parsed.Value);
                    found = true;
                }
            }
        }
        
        return found ? result : null;
    }

    private static TEnum CombineEnum<TEnum>(TEnum left, TEnum right) where TEnum : struct, Enum
    {
        long l = Convert.ToInt64(left);
        long r = Convert.ToInt64(right);
        return (TEnum)Enum.ToObject(typeof(TEnum), l | r);
    }

    /// <summary>
    ///     Parses a string parameter, trimming and stripping surrounding quotes.
    /// </summary>
    /// <param name="parameters">The parsed parameters.</param>
    /// <param name="key">The parameter key.</param>
    /// <returns>The parsed string, or null.</returns>
    public static string? ParseStringParameter(VCardParameters parameters, string key)
    {
        var value = parameters.GetFirst(key);
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim().Trim('"');
    }
}
