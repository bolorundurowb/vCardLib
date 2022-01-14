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

    public Address(string postOfficeBox, string apartmentOrSuiteNumber, string streetAddress, string cityOrLocality,
        string stateOrProvinceOrRegion, string postalOrZipCode, string country)
    {
        PostOfficeBox = postOfficeBox;
        ApartmentOrSuiteNumber = apartmentOrSuiteNumber;
        StreetAddress = streetAddress;
        CityOrLocality = cityOrLocality;
        StateOrProvinceOrRegion = stateOrProvinceOrRegion;
        PostalOrZipCode = postalOrZipCode;
        Country = country;
    }
}