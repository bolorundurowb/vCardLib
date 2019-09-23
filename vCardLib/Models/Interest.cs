using vCardLib.Enums;

namespace vCardLib.Models
{
    /// <summary>
    /// Class to describe a contacts' interest
    /// </summary>
    public class Interest
    {
        /// <summary>
        /// Activity the contact is interested in
        /// </summary>
        public string Activity { get; set; }

        /// <summary>
        /// How proficient the contact is at the activity
        /// </summary>
        public Level Level { get; set; }
    }
}
