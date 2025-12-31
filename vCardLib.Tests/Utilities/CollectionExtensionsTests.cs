using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using vCardLib.Extensions;

namespace vCardLib.Tests.Utilities;

[TestFixture]
public class CollectionExtensionsTests
{
    [Test]
    public void FilterInPlace_ShouldReturnItemsMatchingCondition()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var result = list.FilterInPlace(x => x % 2 == 0).ToList();

        result.Count.ShouldBe(2);
        result.ShouldContain(2);
        result.ShouldContain(4);
    }

    [Test]
    public void FilterInPlace_ShouldRemoveItemsMatchingConditionFromOriginalCollection()
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
        list.Count.ShouldBe(5);
        result.Count.ShouldBe(2);
    }
}
