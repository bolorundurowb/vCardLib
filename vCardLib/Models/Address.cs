using vCardLib.Enums;

namespace vCardLib.Models;

public struct Address
{
    public string PostOfficeBox { get; set; }

    public string ApartmentOrSuiteNumber { get; set; }

    public string StreetAddress { get; set; }

    public string CityOrLocality { get; set; }

    public string StateOrProvinceOrRegion { get; set; }

    public string PostalOrZipCode { get; set; }

    public string Country { get; set; }

    public AddressType Type { get; set; }

    public string? Label { get; set; }

    public Geo? Geographic { get; set; }

    public string? Timezone { get; set; }

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
        Type = addressType != null ? addressType.Value : AddressType.None;
    }
}
