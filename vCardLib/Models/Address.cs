using vCardLib.Enums;

namespace vCardLib.Models;

/// <summary>
/// Represents a vCard address.
/// </summary>
public struct Address
{
    /// <summary>
    /// Gets or sets the Post Office Box number for the address.
    /// </summary>
    public string PostOfficeBox { get; set; }

    /// <summary>
    /// Gets or sets the apartment or suite number for the address.
    /// </summary>
    public string ApartmentOrSuiteNumber { get; set; }

    /// <summary>
    /// Gets or sets the street address for the address.
    /// </summary>
    public string StreetAddress { get; set; }

    /// <summary>
    /// Gets or sets the city or locality for the address.
    /// </summary>
    public string CityOrLocality { get; set; }

    /// <summary>
    /// Gets or sets the state, province, or region for the address.
    /// </summary>
    public string StateOrProvinceOrRegion { get; set; }

    /// <summary>
    /// Gets or sets the postal or zip code for the address.
    /// </summary>
    public string PostalOrZipCode { get; set; }

    /// <summary>
    /// Gets or sets the country for the address.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Gets or sets the type of the address (e.g., Home, Work).
    /// </summary>
    public AddressType Type { get; set; }

    /// <summary>
    /// Gets or sets a label for the address.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the geographical location associated with the address.
    /// </summary>
    public Geo? Geographic { get; set; }

    /// <summary>
    /// Gets or sets the timezone associated with the address.
    /// </summary>
    public string? Timezone { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Address"/> structure.
    /// </summary>
    /// <param name="postOfficeBox">The Post Office Box number for the address.</param>
    /// <param name="apartmentOrSuiteNumber">The apartment or suite number for the address.</param>
    /// <param name="streetAddress">The street address for the address.</param>
    /// <param name="cityOrLocality">The city or locality for the address.</param>
    /// <param name="stateOrProvinceOrRegion">The state, province, or region for the address.</param>
    /// <param name="postalOrZipCode">The postal or zip code for the address.</param>
    /// <param name="country">The country for the address.</param>
    /// <param name="addressType">The type of the address (optional, defaults to None).</param>
    /// <param name="label">A label for the address (optional).</param>
    /// <param name="geographic">The geographical location associated with the address (optional).</param>
    /// <param name="timezone">The timezone associated with the address (optional).</param>
    public Address(string postOfficeBox, string apartmentOrSuiteNumber, string streetAddress, string cityOrLocality,
        string stateOrProvinceOrRegion, string postalOrZipCode, string country, AddressType? addressType = null,
        string? label = null, Geo? geographic = null, string? timezone = null)
    {
        PostOfficeBox = postOfficeBox;
        ApartmentOrSuiteNumber = apartmentOrSuiteNumber;
        StreetAddress = streetAddress;
        CityOrLocality = cityOrLocality;
        StateOrProvinceOrRegion = stateOrProvinceOrRegion;
        PostalOrZipCode = postalOrZipCode;
        Country = country;
        Label = label;
        Geographic = geographic;
        Timezone = timezone;
        Type = addressType ?? AddressType.None;
    }
}