using vCardLib.Enums;

namespace vCardLib.Models;

/// <summary>
/// Represents a label associated with data in a vCard.
/// </summary>
public struct Label
{
    /// <summary>
    /// Gets or sets the text of the label.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the type of address this label refers to (e.g., HOME, WORK).
    /// </summary>
    public AddressType Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Label"/> structure.
    /// </summary>
    /// <param name="text">The text of the label (optional).</param>
    /// <param name="addressType">The type of address this label refers to (optional, defaults to None).</param>
    public Label(string? text, AddressType? addressType = null)
    {
        Text = text;
        Type = addressType ?? AddressType.None;
    }
}