# vCardLib

[![Build status](https://ci.appveyor.com/api/projects/status/3olgly7hvi6vfnsu/branch/master?svg=true)](https://ci.appveyor.com/project/BolorunduroWinnerTimothy/vcardlib/branch/master)
  [![Coverage Status](https://coveralls.io/repos/github/bolorundurowb/vCardLib/badge.svg?branch=master)](https://coveralls.io/github/bolorundurowb/vCardLib?branch=master)    [![NET Standard](https://img.shields.io/badge/netstandard-2.0-ff66b6.svg)]() [![NuGet Badge](https://buildstats.info/nuget/vcardlib.dll)](https://www.nuget.org/packages/vCardLib.dll) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

<br/>

**NOTE: A major redesign is coming using current C# features and with stricter compliance with the RFC-6350 and RFC-2426
standards. feel free to check
the [massive-rewrite-v5](https://github.com/bolorundurowb/vCardLib/tree/feature/massive-rewrite-v5) branch out**

<br/>

This `master` branch contains the latest changes and features (which may be breaking)  to see the last major release (
v3) click [here](https://github.com/bolorundurowb/vCardLib/tree/v3)

T this library supports reading multiple contacts from a single vcf file, a stream or a contact string and returns the
contact objects in a List. The library currently **supports only vCard version 2.1 and 3.0** (a curated list of
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
var contacts = Deserializer.FromFile(filePath);
```

### Deserialize from a Stream

 ```csharp
var stream = // generate stream containing serialized vcards
var contacts = Deserializer.FromStream(stream);
 ```

### Deserialize from a string

 ```csharp
var contactDetails = @"BEGIN:VCARD
VERSION:2.1
N:John;Doe;;;
END:VCARD";
var contacts = Deserializer.FromString(contactDetails);
 ```

All deserialization produces a `vCardCollection` containing the successfully deserialized contact information that can be iterated over. Iterate over the contact collection and pick the vCard objects:

```csharp
foreach(var contact in contacts)
{
  Console.WriteLine(contact.FormattedName);
}
```

## For Serialization

### Serialize as string

```csharp
var vcard = new vCard
{
    Version = vCardVersion.V2,
    FormattedName = "John Doe",
    BirthPlace = "Antarctica",
    Gender = GenderType.Other,
    GivenName = "John",
    MiddleName = "Adekunle",
    FamilyName = "Doe"
};
var serialized = Serializer.Serialize(vcard);

/*
BEGIN:VCARD
VERSION:2.1
REV:20230719T001838Z
N:Doe;John;Adekunle;;
FN:John Doe
BIRTHPLACE:Antarctica
KIND:Individual
GENDER:Other
END:VCARD
 */
```

### Serialize to a Stream

```csharp
var ms = new MemoryStream();
var vcard = new vCard
{
    Version = vCardVersion.V2,
    FormattedName = "John Doe",
    BirthPlace = "Antarctica",
    Gender = GenderType.Other,
    GivenName = "John",
    MiddleName = "Adekunle",
    FamilyName = "Doe"
};
await Serializer.SerializeToStream(vcard, ms);
```

There is support for serializing a collection of `vCard` as well with overloads to the `Serialize` and `SerializeToStream` methods.

## Contributors

A huge thank you to these wonderful people who took time to contribute to this project.

<br/>

[@bolorundurowb](https://github.com/bolorundurowb), [@crowar](https://github.com/crowar)
, [@rmja](https://github.com/rmja), [@JeanCollas](https://github.com/JeanCollas)
