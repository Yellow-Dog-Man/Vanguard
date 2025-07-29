using System.Reflection;

namespace Vanguard;

/// <summary>
/// Wrapper around a single command argument that holds all information necessary to parse it
/// </summary>
public class Argument
{
	/// <summary>
	/// Name of the argument
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// Type of the argument
	/// </summary>
	public Type Type { get; init; }

	/// <summary>
	/// Default value for the argument
	/// </summary>
	public object? DefaultValue { get; init; }

	/// <summary>
	/// Creates a new Argument using reflection to obtain all information for it from a given method parameter
	/// </summary>
	/// <param name="param"></param>
	/// <exception cref="ArgumentException"></exception>
	public Argument(ParameterInfo param)
	{
		// Validate the parameter
		if (param.Name == null)
			throw new ArgumentException($"{param.Member.DeclaringType?.Name}.{param.Member.Name} parameter #{param.Position} has no name.");

		if (param.IsOut)
			throw new ArgumentException($"{param.Member.DeclaringType?.Name}.{param.Member.Name}'s {param.Name} is an output parameter, which is unsupported in command methods");

		if (param.IsRetval)
			throw new ArgumentException($"{param.Member.DeclaringType?.Name}.{param.Member.Name}'s {param.Name} is a retval parameter, which is unsupported in command methods");

		Name = param.Name;
		Type = param.ParameterType;
		DefaultValue = param.DefaultValue;
	}
}
