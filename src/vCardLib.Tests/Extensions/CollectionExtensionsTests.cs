using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Extensions;

namespace vCardLib.Tests.Extensions;

[TestFixture]
public class CollectionExtensionsTests
{
    [Test]
    public void FilterInPlace_ReturnsItemsMatchingCondition()
    {
        var source = new List<int> { 1, 2, 3, 4, 5 };
        var matched = source.FilterInPlace(x => x % 2 == 0).OrderBy(x => x).ToList();
        matched.Must().BeSequenceEqual(new List<int> { 2, 4 });
    }

    [Test]
    public void FilterInPlace_NoMatches_ReturnsEmptySet()
    {
        var source = new List<int> { 1, 3, 5 };
        var matched = source.FilterInPlace(x => x > 10).ToList();
        matched.Must().BeEmpty();
    }

    [Test]
    public void FilterInPlace_AllMatch_ReturnsAll()
    {
        var source = new[] { "a", "b" };
        var matched = source.FilterInPlace(_ => true).ToList();
        matched.Count.Must().Be(2);
        matched.Must().Contain("a");
        matched.Must().Contain("b");
    }
}
