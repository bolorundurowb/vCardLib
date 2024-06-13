# vCardLib: A vCard (.vcf) Processing Library

[![NuGet Badge](https://buildstats.info/nuget/vcardlib.dll)](https://www.nuget.org/packages/vCardLib.dll)  [![Coverage Status](https://coveralls.io/repos/github/bolorundurowb/vCardLib/badge.svg?branch=master)](https://coveralls.io/github/bolorundurowb/vCardLib?branch=master)    [![NET Standard](https://img.shields.io/badge/netstandard-1.3-ff66b6.svg)]() [![NET Standard](https://img.shields.io/badge/netstandard-2.0-3f76b1.svg)]() [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

<br/>

This library provides functionality for working with vCard files.

**Features:**

* Read multiple contacts from a single vcf file, stream, or contact string.
* Returns contact data as an `IEnumerable` object for easy iteration.
* Supports reading and writing vCard versions 2.1, 3.0, and 4.0.

**Branches:**

* **master:** This branch contains the latest **breaking changes and features**. For the most recent stable release (v4), please see the [v4 tag](https://github.com/bolorundurowb/vCardLib/tree/v4).

**Important:** The `master` branch may contain unstable code and is not recommended for production use.


**Improvements:**

* **Clarity:** I've rephrased some sentences to be more clear and concise.
* **Structure:** I've added headings and bullet points to improve readability.
* **Branching:** I've clarified the purpose of the `master` branch and provided a link to the latest stable release.
* **Emphasis:** I've bolded "breaking changes and features" to emphasize the potential instability of the `master` branch.

I hope this improved markdown is helpful!

## How to use this library:

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
