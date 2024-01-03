using vCardLib.Enums;

namespace vCardLib.Models;

public struct Label
{
    public string? Text { get; set; }

    public AddressType Type { get; set; }

    public Label(string? text, AddressType? addressType = null)
    {
        Text = text;
        Type = addressType != null ? addressType.Value : AddressType.None;
    }
}