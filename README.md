# vCardLib

![NuGet Version](https://img.shields.io/nuget/v/vCardLib.dll) [![codecov](https://codecov.io/gh/bolorundurowb/vCardLib/graph/badge.svg?token=UCqAPedMyw)](https://codecov.io/gh/bolorundurowb/vCardLib)
[![NET Standard](https://img.shields.io/badge/netstandard-1.3-ff66b6.svg)]() [![NET Standard](https://img.shields.io/badge/netstandard-2.0-3f76b1.svg)]() [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**vCardLib** is a .NET library for reading and writing **vCard (.vcf)** contacts. It targets **.NET Standard 1.3** and **.NET Standard 2.0** and supports vCard **2.1**, **3.0**, and **4.0**.

Documentation site: [bolorundurowb.github.io/vCardLib](https://bolorundurowb.github.io/vCardLib/)

## Deprecation notice

The **.NET Standard 1.3** target is scheduled to be removed in a future major release. Prefer **.NET Standard 2.0** or a current .NET (6+) for new projects.

## Features

- Parse **one or many** contacts from a file path, a `Stream`, or a string (`IEnumerable<vCard>`).
- Serialize a single `vCard` or a collection to a string, with an optional **output version override**.
- **Line unfolding** when reading: a physical line that begins with a space or tab is merged into the previous line (RFC-style folding). For **vCard 2.1**, a line ending with `=` is also merged with the next line (quoted-printable-style continuation).
- Rich `vCard` model: names, formatted name, phones, emails, addresses, photos, categories, dates, gender, kind, URLs, timezone, geo, custom `X-` properties, and more (see `vCardLib.Models`).

## Installation

**Package Manager**

```cmd
Install-Package vCardLib.dll
```

**.NET CLI**

```bash
dotnet add package vCardLib.dll
```

The NuGet package id is **`vCardLib.dll`**. The latest published version is shown on the badge above.

## Usage

```csharp
using vCardLib.Deserialization;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization;
```

### Deserialization

**From a file**

```csharp
IEnumerable<vCard> contacts = vCardDeserializer.FromFile(@"C:\contacts\people.vcf");
```

**From a stream**

```csharp
// Encoding is chosen from a BOM when possible; the stream must be readable and seekable
// so the BOM probe can rewind (see FileDataHelpers.GetEncoding).
IEnumerable<vCard> contacts = vCardDeserializer.FromStream(stream);
```

**From a string**

`FromContent` requires, in order:

1. Non-whitespace input.
2. The text must **start** with `BEGIN:VCARD` and **end** with `END:VCARD` (exact token match; leading or trailing whitespace is not stripped). A **UTF-8 BOM** at the start will cause the begin check to fail unless you remove it first.
3. A `VERSION` property must appear in the payload.

```csharp
var vcf = @"BEGIN:VCARD
VERSION:2.1
N:Doe;John;;;
FN:John Doe
END:VCARD";

IEnumerable<vCard> contacts = vCardDeserializer.FromContent(vcf);
```

### Serialization

Only properties you set are written. Line breaks follow **`Environment.NewLine`** (platform default), not forced CRLF.

**Single card**

```csharp
var card = new vCard(vCardVersion.v2)
{
    FormattedName = "John Doe"
};
string text = vCardSerializer.Serialize(card);
// Example (Windows): lines separated by \r\n
// BEGIN:VCARD
// VERSION:2.1
// FN:John Doe
// END:VCARD
```

**Version override** (emit another vCard version from the same in-memory card)

```csharp
string asV4 = vCardSerializer.Serialize(card, vCardVersion.v4);
```

**Multiple cards**

```csharp
string bundle = vCardSerializer.Serialize(new[] { card1, card2 });
```

## API summary

| Operation | Entry point |
|-----------|----------------|
| Parse file | `vCardDeserializer.FromFile(string path)` |
| Parse stream | `vCardDeserializer.FromStream(Stream stream)` |
| Parse string | `vCardDeserializer.FromContent(string vcf)` |
| Serialize one | `vCardSerializer.Serialize(vCard card, vCardVersion? overrideVersion = null)` |
| Serialize many | `vCardSerializer.Serialize(IEnumerable<vCard> cards, vCardVersion? overrideVersion = null)` |

**Versions:** use enum members `vCardVersion.v2`, `vCardVersion.v3`, and `vCardVersion.v4` (not `V2` / `V4`).

**vCard 2.1 note:** `NICKNAME` is not written for v2 output (not part of RFC 2426); other versions emit it when `NickName` is set.

## Contributing

Repository: [github.com/bolorundurowb/vCardLib](https://github.com/bolorundurowb/vCardLib).

- **`master`** is the default branch and may include breaking changes while work is in progress.
- **Release tags** (for example [v4](https://github.com/bolorundurowb/vCardLib/tree/v4)) point at stable snapshots if you need a fixed reference.

Build and test locally:

```bash
dotnet build
dotnet test
```

### Contributors

Thanks to [@bolorundurowb](https://github.com/bolorundurowb), [@crowar](https://github.com/crowar), [@rmja](https://github.com/rmja), and [@JeanCollas](https://github.com/JeanCollas).

## License

MIT. See [LICENSE](LICENSE).
