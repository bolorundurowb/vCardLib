namespace vCardLib.Models;

/// <summary>
/// Address class holds the address details
/// </summary>
public class Address
{
	/// <summary>
	/// Gets or sets the location.
	/// </summary>
	/// <value>The location.</value>
	public string Location { get; set; }

	/// <summary>
	/// Gets or sets the location type.
	/// </summary>
	/// <value>The type.</value>
	public AddressType Type { get; set; }
}