// Created by Bolorunduro Winner-Timothy on  11/22/2016 at 5:38 AM

using NUnit.Framework;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
	[TestFixture]
	public class PhoneNumberTests
	{
		[Test]
		public void PhoneNumberIsStable()
		{
			Assert.DoesNotThrow(delegate
			{
				var phoneNumber = new PhoneNumber();
				phoneNumber.Number = "089089893333";
				phoneNumber.Type = PhoneNumberType.BBS;
				phoneNumber.Type = PhoneNumberType.Cell;
				phoneNumber.Type = PhoneNumberType.Car;
				phoneNumber.Type = PhoneNumberType.Fax;
				phoneNumber.Type = PhoneNumberType.Home;
				phoneNumber.Type = PhoneNumberType.ISDN;
				phoneNumber.Type = PhoneNumberType.MainNumber;
				phoneNumber.Type = PhoneNumberType.Modem;
				phoneNumber.Type = PhoneNumberType.None;
				phoneNumber.Type = PhoneNumberType.Pager;
				phoneNumber.Type = PhoneNumberType.Text;
				phoneNumber.Type = PhoneNumberType.TextPhone;
				phoneNumber.Type = PhoneNumberType.Video;
				phoneNumber.Type = PhoneNumberType.Voice;
				phoneNumber.Type = PhoneNumberType.Work;
			});
		}
	}
}
