using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace vCardLib.Utilities;

internal static class EnumExtensions
{
    private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> EnumCache = new();

    public static TEnum Parse<TEnum>(string value) where TEnum : struct, Enum
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        var enumType = typeof(TEnum);

        // Get cached enum values or build and cache them
        if (!EnumCache.TryGetValue(enumType, out var enumValues))
        {
            enumValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (var name in Enum.GetNames(enumType))
                enumValues[name] = Enum.Parse(enumType, name);

            EnumCache[enumType] = enumValues;
        }

        // Attempt to get the enum value from the cache
        if (enumValues.TryGetValue(value, out var enumValue))
            return (TEnum)enumValue;

        // If no match, throw an exception
        throw new ArgumentException($"'{value}' is not a valid value for enum {enumType.Name}.");
    }

    public static T[] Values<T>(T value) where T : struct, Enum
    {
        var enumType = typeof(T);
        return Enum.GetValues(enumType)
            .Cast<T>()
            .Where(x => value.HasFlag(x))
            .ToArray();
    }
}