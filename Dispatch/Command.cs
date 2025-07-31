using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;

namespace Vanguard;

/// <summary>
/// Wrapper around a command controller method that holds all necessary information to run it
/// </summary>
public class Command
{
	/// <summary>
	/// Primary identifier for the command
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// Additional identifiers that are valid to refer to the command with
	/// </summary>
	public IReadOnlySet<string> Aliases { get; init; }

	/// <summary>
	/// Controller type the command method is contained within
	/// </summary>
	public Type Controller { get; init; }

	/// <summary>
	/// Method that the command runs
	/// </summary>
	public MethodInfo Method { get; init; }

	/// <summary>
	/// Argument types to parse and pass to the command method
	/// </summary>
	public IReadOnlyList<Argument> Arguments { get; init; }

	/// <summary>
	/// Creates a new Command using reflection to obtain all information for it from a given controller method
	/// </summary>
	/// <param name="method"></param>
	/// <param name="controller">Controller type the method is for, inferred from the method's declaring type if not provided</param>
	/// <exception cref="ArgumentException"></exception>
	public Command(MethodInfo method, Type? controller = null)
	{
		// Ensure we know the controller type the method is from
		if (controller != null)
		{
			if (!(method.DeclaringType?.IsAssignableTo(controller) ?? false))
				throw new ArgumentException($"{method.DeclaringType}.{method.Name} is not declared by controller {controller.GetType().Name}");
		}
		else
		{
			if (method.DeclaringType == null)
				throw new ArgumentException($"Cannot infer controller type for {method.Name} as its DeclaringType is null");

			controller = method.DeclaringType;
		}

		// Validate the method
		if (method.ContainsGenericParameters)
			throw new ArgumentException($"{method.DeclaringType}.{method.Name} contains generic parameters, which are unsupported in command methods.");

		var commandAttr = method.GetCustomAttribute<CommandAttribute>();

		Controller = controller;
		Method = method;
		Name = commandAttr?.Name ?? method.Name;
		Aliases = commandAttr?.Aliases?.ToFrozenSet(StringComparer.OrdinalIgnoreCase) ?? FrozenSet<string>.Empty;
		Arguments = method.GetParameters().Select(Argument.Create).ToImmutableArray();
	}
}
