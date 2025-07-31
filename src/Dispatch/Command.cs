using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using Vanguard.Framework;

namespace Vanguard.Dispatch;

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
	/// Gets the number of arguments that are optional
	/// </summary>
	public int OptionalArgumentCount => Arguments.Where(a => a.IsOptional).Count();

	/// <summary>
	/// Gets the range of argument counts the command can accept
	/// </summary>
	public (int, int) ValidArgumentCount => (Arguments.Count - OptionalArgumentCount, Arguments.Count);

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

	/// <summary>
	/// Gets the arguments to use for a provided number of values
	/// </summary>
	/// <param name="valCount"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentCountException">When <c>valCount</c> is out of range of <see cref="ValidArgumentCount"/></exception>
	public IEnumerable<Argument> GetArgumentsToUse(int valCount)
	{
		var (minArgs, maxArgs) = ValidArgumentCount;
		if (valCount < minArgs || valCount > maxArgs)
			throw new ArgumentCountException(this, valCount);

		return Arguments.Take(valCount);
	}

	/// <summary>
	/// Indicates that not enough or too many values were supplied to a command's execution
	/// </summary>
	/// <param name="command"></param>
	/// <param name="valCount"></param>
	public class ArgumentCountException(Command command, int valCount)
		: ArgumentOutOfRangeException(BuildMessage(command, valCount))
	{
		/// <summary>
		/// Command that the arguments were being supplied to
		/// </summary>
		public Command Command { get; init; } = command;

		/// <summary>
		/// Number of arguments that were supplied
		/// </summary>
		public int SuppliedValueCount = valCount;

		/// <summary>
		/// Builds a message for the exception
		/// </summary>
		/// <param name="command"></param>
		/// <param name="valCount"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">When <c>valCount</c> is within <see cref="Command.ValidArgumentCount"/></exception>
		private static string BuildMessage(Command command, int valCount)
		{
			var (minArgs, maxArgs) = command.ValidArgumentCount;

			if (valCount < minArgs)
				return $"Not enough values provided to the {command.Name} command; expected between {minArgs} and {maxArgs}, but got {valCount}";

			if (valCount > maxArgs)
				return $"Too many values provided to the {command.Name} command; expected between {minArgs} and {maxArgs}, but got {valCount}";

			throw new ArgumentOutOfRangeException($"Argument count is within the {command.Name} command's expected range");
		}
	}
}
