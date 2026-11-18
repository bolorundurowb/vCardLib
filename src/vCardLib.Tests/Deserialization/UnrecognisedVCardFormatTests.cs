using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization;

namespace vCardLib.Tests.Deserialization;

/// <summary>
/// Regression tests for vCard files produced by phones and other address-book
/// programs that were previously rejected or mis-parsed (issue #81).
/// </summary>
[TestFixture]
public class UnrecognisedVCardFormatTests
{
    [Test]
    public void FromContent_WhenWrappedBase64Photo_ReturnsConcatenatedData()
    {
        const string firstLine = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAYEBQYFBAYGBQYHBwYIChAKCgkJ";
        const string secondLine = "ChQODwwQFxQYGBcUFhYaHSUfGhsjHBYWICwgIyYnKSopGR8tMC0oMCUoKSj/";

        var content = "BEGIN:VCARD\n" +
                      "VERSION:2.1\n" +
                      "FN:Wrapped Photo\n" +
                      "N:Photo;;;\n" +
                      $"PHOTO;JPEG;ENCODING=BASE64:\n{firstLine}\n{secondLine}\n" +
                      "END:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Photos.Count.Must().Be(1);
        card.Photos[0].Encoding.Must().Be("BASE64");
        card.Photos[0].Data.Must().Be(firstLine + secondLine);
        card.CustomFields.Count.Must().Be(0);
    }

    [Test]
    public void FromContent_WhenWrappedBase64Key_DoesNotLeakContinuationLines()
    {
        const string firstLine = "MIILNDCCCRygAwIBAgIKFHTl6AAEAAAXMjANBgkqhkiG9w0BAQUFADBNMRUwEwYK";
        const string secondLine = "CZImiZPyL9QBGRYFbG9jYWwxGDAWBgoJkiaJk/IsZAEZFghzZWN1cml0eTEaMBgG";

        var content = "BEGIN:VCARD\n" +
                      "VERSION:2.1\n" +
                      "FN:Wrapped Key\n" +
                      "N:Key;;;\n" +
                      $"KEY;X509;ENCODING=BASE64:\n{firstLine}\n{secondLine}\n" +
                      "END:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        // Continuation lines must be folded back into the KEY property rather than
        // being surfaced as unknown custom fields.
        card.CustomFields.Count.Must().Be(0);
    }

    [Test]
    public void FromContent_WhenQuotedPrintableName_DecodesValue()
    {
        var content = "BEGIN:VCARD\n" +
                      "VERSION:2.1\n" +
                      "N;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:=D0=92=D0=B0=D0=B4=D0=B8=D0=BC;;;\n" +
                      "FN;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:=D0=92=D0=B0=D0=B4=D0=B8=D0=BC\n" +
                      "END:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Name.Must().NotBeNull();
        card.Name!.Value.FamilyName.Must().Be("Вадим");
        card.FormattedName.Must().Be("Вадим");
    }

    [Test]
    public void FromContent_WhenMultiLineQuotedPrintableName_DecodesValue()
    {
        var content = "BEGIN:VCARD\n" +
                      "VERSION:2.1\n" +
                      "N;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:=D0=92=D0=B0=D0=B4=D0=B8=\n" +
                      "=D0=BC;;;\n" +
                      "END:VCARD";

        var card = vCardDeserializer.FromContent(content).Single();

        card.Name!.Value.FamilyName.Must().Be("Вадим");
    }

    [Test]
    public void FromStream_WhenWindows1251Charset_DecodesName()
    {
        RegisterCodePages();

        var content = "BEGIN:VCARD\n" +
                      "VERSION:2.1\n" +
                      "N;CHARSET=Windows-1251:Иванова;Екатерина;;;\n" +
                      "FN;CHARSET=Windows-1251:Иванова Екатерина\n" +
                      "END:VCARD";

        using var stream = new MemoryStream(Encoding.GetEncoding(1251).GetBytes(content));

        var card = vCardDeserializer.FromStream(stream).Single();

        card.Name!.Value.FamilyName.Must().Be("Иванова");
        card.Name!.Value.GivenName.Must().Be("Екатерина");
        card.FormattedName.Must().Be("Иванова Екатерина");
    }

    [Test]
    public void FromFile_WhenWindows1251Charset_DecodesName()
    {
        RegisterCodePages();

        var content = "BEGIN:VCARD\n" +
                      "VERSION:2.1\n" +
                      "N;CHARSET=Windows-1251:Иванова;Екатерина;;;\n" +
                      "FN;CHARSET=Windows-1251:Иванова Екатерина\n" +
                      "END:VCARD";

        var tempPath = Path.Combine(Path.GetTempPath(), $"vcard-1251-{Guid.NewGuid():N}.vcf");
        try
        {
            File.WriteAllBytes(tempPath, Encoding.GetEncoding(1251).GetBytes(content));

            var card = vCardDeserializer.FromFile(tempPath).Single();

            card.Name!.Value.FamilyName.Must().Be("Иванова");
            card.FormattedName.Must().Be("Иванова Екатерина");
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

    private static void RegisterCodePages()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
}
