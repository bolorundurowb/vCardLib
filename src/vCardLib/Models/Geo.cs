namespace vCardLib.Models;

/// <summary>
/// Holds the Location properties
/// </summary>
public struct Geo(float latitude, float longitude)
{
    /// <summary>
    /// The longitude of the location
    /// </summary>
    public float Longitude { get; set; } = longitude;

    /// <summary>
    /// The latitude of the location
    /// </summary>
    public float Latitude { get; set; } = latitude;
}