using System.Collections.Generic;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization;

// ReSharper disable once InconsistentNaming
public static class vCardSerializer
{
    private static readonly Dictionary<string, IFieldSerializer> FieldSerializers;

    static vCardSerializer()
    {
        var customSerializer = new CustomFieldSerializer();
        var addressSerializer = new AddressFieldSerializer();
        var agentSerializer = new AgentFieldSerializer();
        var anniversarySerializer = new AnniversaryFieldSerializer();
        var birthdaySerializer = new BirthdayFieldSerializer();
        var categoriesSerializer = new CategoriesFieldSerializer();
        var emailAddressSerializer = new EmailAddressFieldSerializer();
        var formattedNameSerializer = new FormattedNameSerializer();

        FieldSerializers = new Dictionary<string, IFieldSerializer>
        {
            { customSerializer.FieldKey, customSerializer },
            { addressSerializer.FieldKey, addressSerializer },
            { agentSerializer.FieldKey, agentSerializer },
            { anniversarySerializer.FieldKey, anniversarySerializer },
            { birthdaySerializer.FieldKey, birthdaySerializer },
            { categoriesSerializer.FieldKey, categoriesSerializer },
            { emailAddressSerializer.FieldKey, emailAddressSerializer },
            { formattedNameSerializer.FieldKey, formattedNameSerializer },
        };
    }
}
