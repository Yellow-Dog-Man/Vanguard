namespace Vanguard;

/// <summary>
/// Marks a method for registration as a Vanguard command
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class CommandAttribute : Attribute
{
	/// <summary>
	/// Primary identifier to register the command with
	/// </summary>
	public string? Name { get; init; }

	/// <summary>
	/// Additional identifiers to register for the command
	/// </summary>
	public string[]? Aliases { get; init; }
}
