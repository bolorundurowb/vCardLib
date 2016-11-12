using NUnit.Framework;

namespace vCardLib.Tests
{
	[TestFixture]
	public class Photos
	{
		[Test]
		public void WhenPictureIsNull()
		{
			Photo photo = new Photo();
			photo.Type = PhotoType.URL;
			photo.Encoding = PhotoEncoding.GIF;
			photo.PhotoURL = "http://google.com/test.gif";
			photo.Picture = null;

			Assert.AreEqual("", photo.ToBase64String());
		}

		[Test]
		public void WhenPictureIsNotNull()
		{
			Photo photo = new Photo();
			photo.Type = PhotoType.Image;
			photo.Encoding = PhotoEncoding.JPEG;
			photo.Picture = new System.Drawing.Bitmap("lib-icon.ico");

			Assert.DoesNotThrow(delegate { photo.ToBase64String(); });
			Assert.Greater(photo.ToBase64String().Length, 0);
		}
	}
}
