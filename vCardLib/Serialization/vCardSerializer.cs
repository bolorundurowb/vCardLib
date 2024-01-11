using System.Collections.Generic;
using System.Linq;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization;

// ReSharper disable once InconsistentNaming
public static class vCardSerializer
{
    private static readonly Dictionary<string, IFieldSerializer> FieldSerializers;

    static vCardSerializer()
    {
        var serializers = new List<IFieldSerializer>()
        {
            new CustomFieldSerializer(),
            new AddressFieldSerializer(),
            new AgentFieldSerializer(),
            new AnniversaryFieldSerializer(),
            new BirthdayFieldSerializer(),
            new CategoriesFieldSerializer(),
            new EmailAddressFieldSerializer(),
            new FormattedNameSerializer(),
            new GenderFieldSerializer(),
        };
        FieldSerializers = serializers.ToDictionary(x => x.FieldKey, y => y);
    }
}
