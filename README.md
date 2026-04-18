# vCardLib

![NuGet Version](https://img.shields.io/nuget/v/vCardLib.dll) [![codecov](https://codecov.io/gh/bolorundurowb/vCardLib/graph/badge.svg?token=UCqAPedMyw)](https://codecov.io/gh/bolorundurowb/vCardLib)
[![NET Standard](https://img.shields.io/badge/netstandard-1.3-ff66b6.svg)]() [![NET Standard](https://img.shields.io/badge/netstandard-2.0-3f76b1.svg)]() [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

**vCardLib** is a .NET library for reading and writing **vCard (.vcf)** contacts. It supports vCard **2.1**, **3.0**, and **4.0** and exposes a small API: deserialize from a file, stream, or string into `vCard` models, and serialize back to text.

Targets **.NET Standard 1.3** and **.NET Standard 2.0**.

## Deprecation notice

The **.NET Standard 1.3** target will be removed in a future major release. New work should target **.NET Standard 2.0** or a current .NET (Core) version.

## Features

- Read one or many contacts from a `.vcf` file, a `Stream`, or a `string` (`IEnumerable<vCard>`).
- Serialize single cards or collections to a string.
- vCard **2.1**, **3.0**, and **4.0** (override output version when serializing if needed).
- **RFC 6350 §3.2** line handling: folded input lines are **unfolded** when reading; output uses **CRLF** delimiters and **folds** long lines (75 UTF-8 octets per physical line) when writing.

## Installation

**Package Manager**

```cmd
Install-Package vCardLib.dll
```

**.NET CLI**

```bash
dotnet add package vCardLib.dll
```

## Usage

Add the namespaces you need:

```csharp
using vCardLib.Deserialization;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization;
```

### Deserialization

**From a file**

```csharp
string filePath = /* path to .vcf */;
IEnumerable<vCard> contacts = vCardDeserializer.FromFile(filePath);
```

**From a stream**

```csharp
Stream stream = /* stream containing vCard text */;
IEnumerable<vCard> contacts = vCardDeserializer.FromStream(stream);
```

**From a string**

```csharp
var vcf = @"BEGIN:VCARD
VERSION:2.1
N:John;Doe;;;
END:VCARD";
IEnumerable<vCard> contacts = vCardDeserializer.FromContent(vcf);
```

Leading/trailing whitespace on the whole document is ignored for the required `BEGIN:VCARD` / `END:VCARD` checks. Folded lines in the source (including LF-only files that use `\n` + continuation space) are merged before properties are parsed.

### Serialization

**Single card**

```csharp
var card = new vCard(vCardVersion.v2)
{
    FormattedName = "John Doe"
};
string serialized = vCardSerializer.Serialize(card);
```

Serialized text uses **CRLF** (`\r\n`) line endings and folds long property lines per RFC 6350.

**Version override**

```csharp
var card = new vCard(vCardVersion.v2)
{
    FormattedName = "John Doe"
};
string serialized = vCardSerializer.Serialize(card, vCardVersion.v4);
```

**Multiple cards**

```csharp
var cards = new[] { card1, card2 };
string serialized = vCardSerializer.Serialize(cards);
```

## API overview

| Task | Type / method |
|------|----------------|
| Parse file | `vCardDeserializer.FromFile(string path)` |
| Parse stream | `vCardDeserializer.FromStream(Stream stream)` |
| Parse string | `vCardDeserializer.FromContent(string vcf)` |
| Serialize one | `vCardSerializer.Serialize(vCard card, vCardVersion? overrideVersion = null)` |
| Serialize many | `vCardSerializer.Serialize(IEnumerable<vCard> cards, vCardVersion? overrideVersion = null)` |
| Contact model | `vCard` (see `vCardLib.Models`) |
| Version enum | `vCardVersion.v2`, `v3`, `v4` |

## Contributing

Source and issues: [github.com/bolorundurowb/vCardLib](https://github.com/bolorundurowb/vCardLib).

- **`master`**: active development; may include breaking changes.
- **Tagged releases** (e.g. [v4](https://github.com/bolorundurowb/vCardLib/tree/v4)): use these for production if you need a stable reference.

To work on the library locally, clone the repository, open the solution, and run tests:

```bash
dotnet test
```

### Contributors

Thanks to everyone who has contributed, including [@bolorundurowb](https://github.com/bolorundurowb), [@crowar](https://github.com/crowar), [@rmja](https://github.com/rmja), and [@JeanCollas](https://github.com/JeanCollas).

## License

MIT. See [LICENSE](LICENSE).
