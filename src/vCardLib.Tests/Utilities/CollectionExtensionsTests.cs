using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Extensions;

namespace vCardLib.Tests.Utilities;

[TestFixture]
public class CollectionExtensionsTests
{
    [Test]
    public void FilterInPlace_ValidInput_ReturnsItemsMatchingCondition()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.FilterInPlace(x => x % 2 == 0).ToList();

        result.Count.Must().Be(2);
        result.Must().Contain(2);
        result.Must().Contain(4);
    }

    [Test]
    public void FilterInPlace_ListInput_DoesNotModifyOriginalCollection()
    {
        // Note: FilterInPlace in the current implementation does NOT actually modify the input IEnumerable
        // unless it's a List<T> and it's casted back, but the implementation does .ToList() first.
        // Let's re-examine the implementation.
        /*
        public static IEnumerable<T> FilterInPlace<T>(this IEnumerable<T> enumerable, Func<T, bool> condition)
        {
            var collection = enumerable.ToList();
            var hashSet = new HashSet<T>(collection.Where(condition));
            collection.RemoveAll(hashSet.Contains);
            return hashSet;
        }
        */
        // The implementation creates a NEW list via .ToList(), so it doesn't filter "in place" on the original enumerable.
        // It returns the filtered out items.

        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.FilterInPlace(x => x % 2 == 0).ToList();

        // The original 'list' remains unchanged because .ToList() created a copy.
        list.Count.Must().Be(5);
        result.Count.Must().Be(2);
    }
}
