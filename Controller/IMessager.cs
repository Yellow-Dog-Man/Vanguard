namespace Vanguard;

/// <summary>
/// Sends messages
/// </summary>
public interface IMessager
{
	/// <summary>
	/// Sends a message for a command context
	/// </summary>
	/// <param name="context"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	public Task Send(ICommandContext context, string message);
}
