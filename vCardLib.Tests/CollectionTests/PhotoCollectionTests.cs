using System;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Models;

namespace vCardLib.Tests.CollectionTests
{
    [TestFixture]
    class PhotoCollectionTests
    {
        [Test]
        public void InsertAndRemove()
        {
            Assert.DoesNotThrow(delegate {
                var photo = new Photo();
                var photoCollection = new PhotoCollection();
                photoCollection.Add(photo);
                photoCollection[0] = photo;
                photo = photoCollection[0];
				photoCollection.Remove(photo);
            });
        }

        [Test]
        public void InsertAndRemoveNonExistentIndices()
        {
            var photoCollection = new PhotoCollection();
            Assert.Throws<IndexOutOfRangeException>(delegate
            {
                var photo_ = photoCollection[0];
            });
            var photo = new Photo();
            Assert.Throws<IndexOutOfRangeException>(delegate
            {
                photoCollection[0] = photo;
            });
        }
    }
}
