using System;
using System.Collections.Generic;
using System.Linq;
using vCardLib.Constants;

namespace vCardLib.Deserialization.Utilities;

/// <summary>
///     Encapsulates vCard field parameters (metadata).
/// </summary>
internal sealed class VCardParameters
{
    private readonly Dictionary<string, List<string?>> _parameters = new(StringComparer.OrdinalIgnoreCase);

    private VCardParameters()
    {
    }

    /// <summary>
    ///     Returns all values for the given key, or empty.
    /// </summary>
    /// <param name="key">The parameter key.</param>
    /// <returns>A collection of values.</returns>
    public IEnumerable<string?> GetAll(string key)
    {
        return _parameters.TryGetValue(key, out var values) ? values : Enumerable.Empty<string?>();
    }

    /// <summary>
    ///     Returns the first value for the given key, or null.
    /// </summary>
    /// <param name="key">The parameter key.</param>
    /// <returns>The first value, or null.</returns>
    public string? GetFirst(string key)
    {
        return _parameters.TryGetValue(key, out var values) ? values.FirstOrDefault() : null;
    }

    /// <summary>
    ///     Checks if the given key exists (case-insensitive).
    /// </summary>
    /// <param name="key">The parameter key.</param>
    /// <returns>True if the key exists, else false.</returns>
    public bool ContainsKey(string key)
    {
        return _parameters.ContainsKey(key);
    }

    /// <summary>
    ///     Gets all keys.
    /// </summary>
    public IEnumerable<string> Keys => _parameters.Keys;

    /// <summary>
    ///     Parses the metadata array into a <see cref="VCardParameters"/> instance.
    /// </summary>
    /// <param name="metadata">The metadata array from <see cref="DataSplitHelpers.SplitLine"/>.</param>
    /// <returns>A <see cref="VCardParameters"/> instance.</returns>
    public static VCardParameters Parse(string[]? metadata)
    {
        var parameters = new VCardParameters();
        if (metadata == null || metadata.Length == 0)
        {
            return parameters;
        }

        foreach (var datum in metadata)
        {
            if (string.IsNullOrWhiteSpace(datum))
            {
                continue;
            }

            var (key, value) = DataSplitHelpers.SplitDatum(datum, FieldKeyConstants.KeyValueDelimiter);
            
            // For entries that are bare tokens (no =), treat the token as both the key and the value
            value ??= key;

            if (!parameters._parameters.TryGetValue(key, out var values))
            {
                values = new List<string?>();
                parameters._parameters[key] = values;
            }
            values.Add(value);
        }

        return parameters;
    }
}
