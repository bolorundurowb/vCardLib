# vCardLib

[![NuGet Badge](https://buildstats.info/nuget/vcardlib.dll)](https://www.nuget.org/packages/vCardLib.dll)  [![Coverage Status](https://coveralls.io/repos/github/bolorundurowb/vCardLib/badge.svg?branch=master)](https://coveralls.io/github/bolorundurowb/vCardLib?branch=master)    [![NET Standard](https://img.shields.io/badge/netstandard-1.3-ff66b6.svg)]() [![NET Standard](https://img.shields.io/badge/netstandard-2.0-3f76b1.svg)]() [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

<br/>

This `master` branch contains the latest changes and features (which are breaking)  to see the last major release (
v4) click [here](https://github.com/bolorundurowb/vCardLib/tree/v4)

This library supports reading multiple contacts from a single vcf file, a stream or a contact string and returns the
contact objects in an `Enumerable`. The library currently **supports only vCard version 2.1 and 3.0** (a curated list of
properties supported can be seen on the documentation site).

#### How to use the library:

First get this package from nuget via your package manager:

```
Install-Package vCardLib.dll
```

or

```bash
dotnet add package vCardLib.dll
```

## For Deserialization

### Deserialize from a file

```csharp
string filePath = // path to vcf file;
IEnumerable<vCard> contacts = vCardDeserializer.FromFile(filePath);
```

### Deserialize from a Stream

 ```csharp
var stream = // generate stream containing serialized vcards
IEnumerable<vCard> contacts = vCardDeserializer.FromStream(stream);
 ```

### Deserialize from a string

 ```csharp
var contactDetails = @"BEGIN:VCARD
VERSION:2.1
N:John;Doe;;;
END:VCARD";
IEnumerable<vCard> contacts = vCardDeserializer.FromContent(contactDetails);
 ```

## For Serialization

### Serialize as string

```csharp
var vcard = new vCard(vCardVersion.v2)
{
    FormattedName = "John Doe"
};
var serialized = vCardSerializer.Serialize(vcard);

/*
BEGIN:VCARD
VERSION:2.1
REV:20230719T001838Z
FN:John Doe
END:VCARD
 */
```

### Serialize with an override

This allows a vcard to get serialized to a different version

```csharp
var vcard = new vCard(vCardVersion.v2)
{
    FormattedName = "John Doe"
};
var serialized = vCardSerializer.Serialize(vcard, vCardVersioon.v4);

/*
BEGIN:VCARD
VERSION:4.0
REV:20230719T001838Z
FN:John Doe
END:VCARD
 */
```


## Contributors

A huge thank you to these wonderful people who took time to contribute to this project.

<br/>

[@bolorundurowb](https://github.com/bolorundurowb), [@crowar](https://github.com/crowar)
, [@rmja](https://github.com/rmja), [@JeanCollas](https://github.com/JeanCollas)
