namespace vCardLib.Models;

/// <summary>
/// Holds the Location properties
/// </summary>
public struct Geo
{
    /// <summary>
    /// The longitude of the location
    /// </summary>
    public float Longitude { get; set; }

    /// <summary>
    /// The latitude of the location
    /// </summary>
    public float Latitude { get; set; }

    public Geo(float latitude, float longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
}