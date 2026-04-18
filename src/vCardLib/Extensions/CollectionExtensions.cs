using System;
using System.Collections.Generic;
using System.Linq;

namespace vCardLib.Extensions;

internal static class CollectionExtensions
{
    public static IEnumerable<T> FilterInPlace<T>(this IEnumerable<T> enumerable, Func<T, bool> condition)
    {
        var collection = enumerable.ToList();
        var hashSet = new HashSet<T>(collection.Where(condition));
        collection.RemoveAll(hashSet.Contains);
        return hashSet;
    }
}
