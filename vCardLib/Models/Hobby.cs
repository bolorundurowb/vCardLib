using vCardLib.Enums;

namespace vCardLib.Models;

/// <summary>
/// A hobby, as opposed to an interest,
/// is an activity that one actively engages in for
/// entertainment, intellectual stimulation, creative
/// expression, or the like.
/// </summary>
public class Hobby
{
    /// <summary>
    /// The activity that the contact engages in
    /// </summary>
    public string Activity { get; set; }

    /// <summary>
    /// How proficient the contact is at the activity
    /// </summary>
    public Level Level { get; set; }
}