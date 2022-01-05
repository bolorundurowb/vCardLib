using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers
{
    internal class VersionDeserializer : IFieldDeserializer<float>
    {
        public string FieldKey => "VERSION";

        public float Read(string input)
        {
            var separatorIndex = input.IndexOf(':');
            var representation = input.Substring(separatorIndex + 1).Trim();
            return float.Parse(representation);
        }
    }
}