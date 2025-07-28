namespace Vanguard;

/// <summary>
/// Context for a command usage
/// </summary>
public interface ICommandContext
{
	/// <summary>
	/// User that is running the command
	/// </summary>
	public IUser User { get; init; }

	/// <summary>
	/// Entity the command was triggered by (for example, the message received from the user)
	/// </summary>
	public ICommandTrigger Trigger { get; init; }
}
