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
    public enum OverWriteOptions
    {
        Proceed,
        Throw
    }

    /// <summary>
    /// vCard version
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public enum vCardVersion
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
