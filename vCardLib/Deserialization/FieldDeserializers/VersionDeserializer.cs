using System;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;

namespace vCardLib.Deserialization.FieldDeserializers
{
    internal class VersionDeserializer : IFieldDeserializer, IV2FieldDeserializer<vCardVersion>,
        IV3FieldDeserializer<vCardVersion>, IV4FieldDeserializer<vCardVersion>
    {
        public string FieldKey => "VERSION";

        public vCardVersion Read(string input)
        {
            var separatorIndex = input.IndexOf(':');
            var value = input.Substring(separatorIndex + 1).Trim();

            switch (value)
            {
                case "2.1":
                    return vCardVersion.v2;
                case "3.0":
                    return vCardVersion.v3;
                case "4.0":
                    return vCardVersion.v4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Parsed version is not supported.");
            }
        }
    }
}