using NUnit.Framework;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
    [TestFixture]
    public class GeoTests
    {
        public void PropertiesTest()
        {
            Assert.DoesNotThrow(
                delegate
                {
                    var geo = new Geo()
                    {
                        Latitude = -4.56,
                        Longitude = +45.77
                    };
                });
        }
    }
}