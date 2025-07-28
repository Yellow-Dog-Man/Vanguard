namespace Vanguard;

/// <summary>
/// Mode for applying permission checks
/// </summary>
public enum PermissionsMode
{
	/// <summary>
	/// Require all permissions to be fulfilled
	/// </summary>
	RequireAll,

	/// <summary>
	/// Require any (at least one) permission to be fulfilled
	/// </summary>
	RequireAny,
}
