using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
	[TestFixture]
	public class PhotoTests
	{
		[Test]
		public void WhenPictureIsNull()
		{
			var photo = new Photo {
				Type = PhotoType.URL,
				Encoding = PhotoEncoding.GIF,
				PhotoURL = "http://google.com/test.gif",
				Picture = null};

			Assert.AreEqual("", photo.ToBase64String());
		}

		[Test]
		public async Task WhenPictureIsNotNull()
		{
			var request = System.Net.WebRequest.Create("https://jpeg.org/images/jpeg-logo-plain.png");
            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();

            var photo = new Photo
            {
	            Type = PhotoType.Image,
	            Encoding = PhotoEncoding.JPEG
            };

            using (var memoryStream = new MemoryStream())
            {
	            await responseStream.CopyToAsync(memoryStream);
	            photo.Picture = memoryStream.ToArray();
            }

            Assert.DoesNotThrow(delegate { photo.ToBase64String(); });
			Assert.Greater(photo.ToBase64String().Length, 0);
		}
	}
}
