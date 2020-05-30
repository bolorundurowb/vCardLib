# vCardLib
[![Build status](https://ci.appveyor.com/api/projects/status/3olgly7hvi6vfnsu?svg=true)](https://ci.appveyor.com/project/BolorunduroWinnerTimothy/vcf-reader)  [![Coverage Status](https://coveralls.io/repos/github/bolorundurowb/vCardLib/badge.svg?branch=master)](https://coveralls.io/github/bolorundurowb/vCardLib?branch=master)    [![NET Standard](https://img.shields.io/badge/netstandard-2.0-ff66b6.svg)]() [![NuGet Badge](https://buildstats.info/nuget/vcardlib.dll)](https://www.nuget.org/packages/vCardLib.dll) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Documentation can be found at [documentation site](http://bolorundurowb.github.io/vCardLib/)

This is the library that powers the VCF Reader. Unlike all other vCard libraries for .NET that I found, this library supports reading multiple contacts from a single vcf file and returns the contact objects in a vCardCollection. The library currently **supports only vCard version 2.1 and 3.0** (a curated list of properties supported can be seen on the documentation site).

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

Import the namespaces:
```csharp
using vCardLib.Deserializers;
```

In your class you call the static method 'FromFile' and pass a string containing a path to it:

```csharp
string filePath = //path to vcf file;

vCardCollection contacts = Deserializer.FromFile(filePath);
```
 Or pass a  StreamReader object to it:
 ```csharp
StreamReader sr = //generate a streamreader somehow;
vCardCollection contacts = Deserializer.FromStreamReader(sr);
 ```

Iterate over the collection and pick the vCard objects:

```csharp
foreach(vCard contact in contacts)
{
  Console.WriteLine(contact.FormattedName);
}
```
complete documentation on [github.io](http://bolorundurowb.github.io/vCardLib/)
