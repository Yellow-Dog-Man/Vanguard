namespace Vanguard;

/// <summary>
/// Controller that houses Vanguard command methods, instantiated for each command execution
/// </summary>
public abstract class CommandController<P>(IMessager messager, ICommandContext context)
{
	/// <summary>
	/// IMessager implementation to send messages
	/// </summary>
	public IMessager Messager { get; init; } = messager;

	/// <summary>
	/// Context the command is being used in
	/// </summary>
	public ICommandContext Context { get; init; } = context;

	/// <summary>
	/// Sends a message in response to the command context
	/// </summary>
	/// <param name="message"></param>
	/// <returns></returns>
	protected async Task Respond(string message)
	{
		await Messager.Send(Context, message).ConfigureAwait(false);
	}

	/// <summary>
	/// Checks whether the command user has given permissions
	/// </summary>
	/// <param name="permissions"></param>
	/// <param name="mode"></param>
	/// <returns></returns>
	public abstract ValueTask<bool> HasPermission(P[] permissions, PermissionsMode mode = PermissionsMode.RequireAll);

	/// <summary>
	/// Checks whether the command user has all of the given permissions
	/// </summary>
	/// <param name="permissions"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public async ValueTask<bool> HasPermission(params P[] permissions)
	{
		return await HasPermission(permissions, PermissionsMode.RequireAll).ConfigureAwait(false);
	}

	/// <summary>
	/// Checks whether the command user has any of the given permissions
	/// </summary>
	/// <param name="permissions"></param>
	/// <returns></returns>
	public async ValueTask<bool> HasAnyPermission(params P[] permissions)
	{
		return await HasPermission(permissions, PermissionsMode.RequireAny).ConfigureAwait(false);
	}
}
