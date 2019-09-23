using vCardLib.Enums;

namespace vCardLib.Models
{
    /// <summary>
    /// Holds Expertise properties
    /// </summary>
    public class Expertise
    {
        /// <summary>
        /// The skill in which you are an expert
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// How good are you at said skill
        /// </summary>
        public Level Level { get; set; }
    }
}
