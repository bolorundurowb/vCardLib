using System;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class AnniversaryFieldSerializer : IV2FieldSerializer<DateTime>, IV3FieldSerializer<DateTime>,
    IV4FieldSerializer<DateTime>
{
    public string FieldKey => "ANNIVERSARY";

    public string? Write(DateTime data) => null;

    string? IV4FieldSerializer<DateTime>.Write(DateTime data) => $"{FieldKey}:{data:yyyyMMdd}";
}
