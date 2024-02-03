using System;
using vCardLib.Constants;
using vCardLib.Enums;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class VersionFieldSerializer
{
    public static string FieldKey => "VERSION";

    public static string Write(vCardVersion version)
    {
        var parsedVersion = version switch
        {
            vCardVersion.v2 => "2.1",
            vCardVersion.v3 => "3.0",
            vCardVersion.v4 => "4.0",
            _ => throw new ArgumentOutOfRangeException(nameof(version), version, "Version is not supported.")
        };

        return $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{parsedVersion}";
    }
}
