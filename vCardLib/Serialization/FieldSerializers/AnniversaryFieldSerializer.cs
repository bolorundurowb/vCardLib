using System;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

public class AnniversaryFieldSerializer : IV2FieldSerializer<DateTime?>, IV3FieldSerializer<DateTime?>,
    IV4FieldSerializer<DateTime?>
{
    public string FieldKey => "ANNIVERSARY";

    public string? Write(DateTime? data) => null;

    string? IV4FieldSerializer<DateTime?>.Write(DateTime? data)
    {
        if (data == null)
            return null;

        return $"{FieldKey}: {data:yyyyMMdd}";
    }
}
