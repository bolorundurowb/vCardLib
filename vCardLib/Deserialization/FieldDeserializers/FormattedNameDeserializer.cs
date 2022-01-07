using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers
{
    internal class FormattedNameDeserializer : IFieldDeserializer<string>
    {
        public string FieldKey => "FN";
        
        public string Read(string input)
        {
            var separatorIndex = input.IndexOf(':');
            return input.Substring(separatorIndex + 1).Trim();
        }
    }
}