# vCardLib: A vCard (.vcf) Processing Library 📇

![NuGet Version](https://img.shields.io/nuget/v/vCardLib.dll)  [![Coverage Status](https://coveralls.io/repos/github/bolorundurowb/vCardLib/badge.svg?branch=master)](https://coveralls.io/github/bolorundurowb/vCardLib?branch=master) 
[![NET Standard](https://img.shields.io/badge/netstandard-1.3-ff66b6.svg)]()  [![NET Standard](https://img.shields.io/badge/netstandard-2.0-3f76b1.svg)]()  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

## About vCardLib 📜

**vCardLib** is a powerful and flexible .NET library designed to simplify working with **vCard (.vcf)** files. Whether you're reading, writing, or manipulating contact information, **vCardLib** provides an easy-to-use API to handle vCard versions **2.1, 3.0, and 4.0** seamlessly. 🌟

Perfect for applications dealing with contact management, address books, or any scenario where vCard files are used, **vCardLib** ensures your vCard processing is smooth and efficient. 🚀

---

## Features ✨

- **Read Multiple Contacts**: Parse multiple contacts from a single `.vcf` file, stream, or string.
- **Easy Iteration**: Returns contact data as an `IEnumerable<vCard>` for effortless looping.
- **Cross-Version Support**: Works with vCard versions **2.1, 3.0, and 4.0**.
- **Serialization and Deserialization**: Easily convert between vCard objects and their string/file representations.

---

## Installation 📦

You can install **vCardLib** via NuGet using one of the following methods:

#### **Package Manager**
```cmd
Install-Package vCardLib.dll
```

#### **.NET CLI**
```bash
dotnet add package vCardLib.dll
```

---

## Usage 🛠️

### Deserialization (Reading vCards)

#### **From a File**
```csharp
string filePath = // path to .vcf file;
IEnumerable<vCard> contacts = vCardDeserializer.FromFile(filePath);
```

#### **From a Stream**
```csharp
var stream = // generate stream containing serialized vCards
IEnumerable<vCard> contacts = vCardDeserializer.FromStream(stream);
```

#### **From a String**
```csharp
var contactDetails = @"BEGIN:VCARD
VERSION:2.1
N:John;Doe;;;
END:VCARD";
IEnumerable<vCard> contacts = vCardDeserializer.FromContent(contactDetails);
```

---

### Serialization (Writing vCards)

#### **Serialize as String**
```csharp
var vcard = new vCard(vCardVersion.V2)
{
    FormattedName = "John Doe"
};
var serialized = vCardSerializer.Serialize(vcard);

/*
Output:
BEGIN:VCARD
VERSION:2.1
REV:20230719T001838Z
FN:John Doe
END:VCARD
*/
```

#### **Serialize with Version Override**
```csharp
var vcard = new vCard(vCardVersion.V2)
{
    FormattedName = "John Doe"
};
var serialized = vCardSerializer.Serialize(vcard, vCardVersion.V4);

/*
Output:
BEGIN:VCARD
VERSION:4.0
REV:20230719T001838Z
FN:John Doe
END:VCARD
*/
```

---

## Branches 🌿

- **`master`**: Contains the latest **breaking changes and features**. 🚧  
  **Note:** This branch may contain unstable code and is not recommended for production use.

- **[v4 Tag](https://github.com/bolorundurowb/vCardLib/tree/v4)**: The most recent stable release. ✅

---

## Contributors 🙌

A huge thank you to these amazing contributors who have helped make **vCardLib** better:

[@bolorundurowb](https://github.com/bolorundurowb), [@crowar](https://github.com/crowar),  
[@rmja](https://github.com/rmja), [@JeanCollas](https://github.com/JeanCollas)

---

## License 📜

**vCardLib** is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

---

## Get Started Today! 🎉

Whether you're building a contact management system, integrating vCard support into your app, or just need to process `.vcf` files, **vCardLib** is here to make your life easier. Install the package, follow the examples, and start working with vCards like a pro! ⏱️

**Happy Coding!** 🚀