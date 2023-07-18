// Created by Bolorunduro Winner-Timothy on  11/22/2016 at 5:38 AM

using NUnit.Framework;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests;

[TestFixture]
public class PhoneNumberTests
{
	[Test]
	public void PhoneNumberIsStable()
	{
		Assert.DoesNotThrow(delegate
		{
			var phoneNumber = new TelephoneNumber();
			phoneNumber.Value = "089089893333";
			phoneNumber.Type = TelephoneNumberType.BBS;
			phoneNumber.Type = TelephoneNumberType.Cell;
			phoneNumber.Type = TelephoneNumberType.Car;
			phoneNumber.Type = TelephoneNumberType.Fax;
			phoneNumber.Type = TelephoneNumberType.Home;
			phoneNumber.Type = TelephoneNumberType.ISDN;
			phoneNumber.Type = TelephoneNumberType.MainNumber;
			phoneNumber.Type = TelephoneNumberType.Modem;
			phoneNumber.Type = TelephoneNumberType.None;
			phoneNumber.Type = TelephoneNumberType.Pager;
			phoneNumber.Type = TelephoneNumberType.Text;
			phoneNumber.Type = TelephoneNumberType.TextPhone;
			phoneNumber.Type = TelephoneNumberType.Video;
			phoneNumber.Type = TelephoneNumberType.Voice;
			phoneNumber.Type = TelephoneNumberType.Work;
		});
	}
}