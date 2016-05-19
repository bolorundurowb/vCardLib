# This project contains two sub projects
[![Build status](https://ci.appveyor.com/api/projects/status/sbhyvvpj8jy8ifmw/branch/master?svg=true)](https://ci.appveyor.com/project/BolorunduroWinnerTimothy/vcf-reader/branch/master)   [![SourceForge](https://img.shields.io/badge/downloads-8%2Fwk-brightgreen.svg)](https://sourceforge.net/projects/vcf-reader/) [![NuGet](https://img.shields.io/badge/nuget-1.1.1-blue.svg)](https://www.nuget.org/packages/vCardLib.dll) [![NETFramework](https://img.shields.io/badge/.net-4.5-ff66b6.svg)]()

## VCF-Reader

This tool was developed because I recently lost my android phone but was blessed to have created a VCF backup of all my contacts. VCF Reader loads contacts from a vCard (VCF) file into a table which allows sorting and case insensitive searching. The table shows the Surname, the first name, one email address and two Phone numbers

##vCardLib

This is the library that powers the VCF Reader. Unlike all other vCard libraries for .NET that I found, this library supports reading multiple contacts from a single vcf file and returns the contact objects in a vCardCollection.

How to use the library:

```csharp
string filePath = //path to vcf file

vCardCollection contacts = vCard.FromFile(filePath);
```


iterate over the collection and pick the vCard objects:

```csharp
foreach(vCard contact in contacts)
{
  Console.WriteLine(contact.FullName);
}
```
