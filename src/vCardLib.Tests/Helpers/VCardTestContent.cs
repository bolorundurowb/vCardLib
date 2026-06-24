using System;
using vCardLib.Enums;

namespace vCardLib.Tests.Helpers;

/// <summary>
/// Shared vCard wire-format snippets for deserializer integration tests.
/// </summary>
internal static class VCardTestContent
{
    public static string Minimal(vCardVersion version, string formattedName) =>
        $"BEGIN:VCARD\nVERSION:{ToVersionToken(version)}\nFN:{formattedName}\nEND:VCARD";

    public static string ToVersionToken(vCardVersion version) => version switch
    {
        vCardVersion.v2 => "2.1",
        vCardVersion.v3 => "3.0",
        vCardVersion.v4 => "4.0",
        _ => throw new ArgumentOutOfRangeException(nameof(version), version, "Unsupported vCard version.")
    };
}
