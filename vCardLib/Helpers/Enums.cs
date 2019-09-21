namespace vCardLib.Helpers
{
    /// <summary>
    /// Gender types
    /// </summary>
    public enum GenderType
    {
        Male,
        Female,
        Other,
        None
    }

    /// <summary>
    /// Contact types
    /// </summary>
    public enum ContactType
    {
        Individual,
        Group,
        Organization,
        Location,
        Application,
        Device
    }

    /// <summary>
    /// Available write options
    /// </summary>
    public enum WriteOptions
    {
        Overwrite,
        ThrowError
    }

    /// <summary>
    /// vCard version
    /// </summary>
    public enum VcardVersion
    {
        V2,
        V3,
        V4
    }

	/// <summary>
	/// Various activity levels
	/// </summary>
	public enum Level
	{
		High,
		Medium,
		Low
	}
}
