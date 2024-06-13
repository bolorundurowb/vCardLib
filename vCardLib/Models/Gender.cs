using vCardLib.Enums;

namespace vCardLib.Models
{
    /// <summary>
    /// Represents gender information for a vCard.
    /// </summary>
    public struct Gender
    {
        /// <summary>
        /// Gets or sets the biological sex of the individual (e.g., Male, Female).
        /// </summary>
        public BiologicalSex? Sex { get; set; }

        /// <summary>
        /// Gets or sets the gender identity of the individual.
        /// </summary>
        public string? GenderIdentity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gender"/> structure.
        /// </summary>
        /// <param name="sex">The biological sex of the individual (optional).</param>
        /// <param name="genderIdentity">The gender identity of the individual (optional).</param>
        public Gender(BiologicalSex? sex, string? genderIdentity)
        {
            Sex = sex;
            GenderIdentity = genderIdentity;
        }
    }
}
