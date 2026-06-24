using System.Collections.Generic;
using vCardLib.Models;

namespace vCardLib.Serialization;

internal interface IFieldSerializer
{
    IEnumerable<string>? Serialize(vCard card);
}
