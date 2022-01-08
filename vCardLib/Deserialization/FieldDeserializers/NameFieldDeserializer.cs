using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers
{
    internal class NameFieldDeserializer : IFieldDeserializer,
        IV2FieldDeserializer<(string, string, string, string, string)>,
        IV3FieldDeserializer<(string, string, string, string, string)>,
        IV4FieldDeserializer<(string, string, string, string, string)>
    {
        public string FieldKey => "N";

        public (string, string, string, string, string) Read(string input)
        {
            var separatorIndex = input.IndexOf(':');
            var value = input.Substring(separatorIndex + 1).Trim();
            string familyName = null,
                givenName = null,
                additionalNames = null,
                honorificPrefix = null,
                honorificSuffix = null;

            var nameParts = value.Split(';');
            var partsLength = nameParts.Length;

            if (partsLength > 0)
                familyName = nameParts[0];

            if (partsLength > 1)
                givenName = nameParts[1];

            if (partsLength > 2)
                additionalNames = nameParts[2];

            if (partsLength > 3)
                honorificPrefix = nameParts[1];

            if (partsLength > 4)
                honorificSuffix = nameParts[1];

            return (familyName, givenName, additionalNames, honorificPrefix, honorificSuffix);
        }
    }
}