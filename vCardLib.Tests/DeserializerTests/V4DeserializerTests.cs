﻿using System;
using NUnit.Framework;
using vCardLib.Deserializers;
using vCardLib.Models;

namespace vCardLib.Tests.DeserializerTests
{
    [TestFixture]
    public class V4DeserializerTests
    {
        [Test]
        public void ParseTest()
        {
            Assert.Throws<NotImplementedException>(
                delegate
                {
                    V4Deserializer.Parse(new[] {"test"}, new vCard());
                });
        }
    }
}