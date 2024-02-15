using System;
using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class RevisionFieldSerializer : IV2FieldSerializer<DateTime>, IV3FieldSerializer<DateTime>,
    IV4FieldSerializer<DateTime>
{
    public string FieldKey => "REV";

    public string Write(DateTime data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data:yyyyMMddTHHmmssZ}";
}
