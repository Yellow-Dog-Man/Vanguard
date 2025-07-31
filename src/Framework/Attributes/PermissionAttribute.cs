namespace Vanguard.Framework;

/// <summary>
/// Marks a command method as requiring the specified permission(s) in order to be executed
/// </summary>
/// <typeparam name="P"></typeparam>
/// <param name="permissions"></param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PermissionAttribute<P>(params P[] permissions) : Attribute
{
	/// <summary>
	/// Permissions required to run the command
	/// </summary>
	public P[] Permissions { get; init; } = permissions;

	/// <summary>
	/// Mode to use for checking the permissions
	/// </summary>
	public PermissionsMode Mode { get; init; }
}
