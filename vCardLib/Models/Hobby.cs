using vCardLib.Enums;

namespace vCardLib.Models
{
    /// <summary>
    /// Class to describe a contacts' hobby
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
}
