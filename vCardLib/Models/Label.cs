using System.Collections.Generic;
using vCardLib.Enums;

namespace vCardLib.Models;

public struct Label
{
    public string Text { get; set; }

    public List<AddressType> Types { get; set; } = new();

    public Label(string text) => Text = text;
}