# VCF-Reader
[![Coverage Status](https://coveralls.io/repos/github/bolorundurowb/VCF-Reader/badge.svg?branch=master)](https://coveralls.io/github/bolorundurowb/VCF-Reader?branch=master)

This tool was developed because i recently lost my android phone but was blessed to have created a VCF backup of all my contacts. VCF Reader loads contacts from a vcf file into a table which allows sorting and case insensitive searching. The table shows the Surname, the Firstname, one email address and two Phone numbers

#vCardLib
This is the library that powers the VCF Reader. Unlike all other vCard libraries for .NET that I found, this library supports reading multiple contacts from a single vcf file and returns it in a vCardCollection.

How to use the library:

string filePath = //path to vcf file

vCardCollection vcardCollection = vCard.FromFile(filePath);


iterate over the collection and pick the vCard objects
