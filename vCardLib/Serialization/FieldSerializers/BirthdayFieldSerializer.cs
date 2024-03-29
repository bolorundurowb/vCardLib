﻿using System;
using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class BirthdayFieldSerializer : IV2FieldSerializer<DateTime>, IV3FieldSerializer<DateTime>, IV4FieldSerializer<DateTime>
{
    public string FieldKey => "BDAY";

    public string? Write(DateTime data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data:yyyyMMdd}";
}
