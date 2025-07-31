using System.Reflection;

namespace Vanguard;

/// <summary>
/// Wrapper around a single command argument that holds all information necessary to parse it
/// <typeparam name="T"></typeparam>
/// </summary>
public class Argument<T>
{
	private static NullabilityInfoContext _nullableContext = new();

	/// <summary>
	/// Name of the argument
	/// </summary>
	public string Name { get; init; }

	/// <summary>
	/// Type of the argument
	/// </summary>
	public Type Type { get; } = typeof(T);

	/// <summary>
	/// Whether the argument is optional
	/// </summary>
	public bool IsOptional { get; init; } = false;

	/// <summary>
	/// Default value for the argument
	/// </summary>
	public T? DefaultValue { get; init; } = default;


	/// <summary>
	/// Creates a new Argument using reflection to obtain all information for it from a given method parameter
	/// </summary>
	/// <param name="param"></param>
	/// <exception cref="ArgumentException"></exception>
	public Argument(ParameterInfo param)
	{
		var methodName = $"{param.Member.DeclaringType?.Name}.{param.Member.Name}";
		var underlying = param.ParameterType.IsValueType ? Nullable.GetUnderlyingType(param.ParameterType) : null;

		// Validate the parameter
		if (param.Name == null)
			throw new ArgumentException($"{methodName} parameter #{param.Position} has no name");

		if ((underlying ?? param.ParameterType) != typeof(T))
			throw new ArgumentException($"{methodName}'s {param.Name} parameter type ({param.ParameterType.FullName}) doesn't match this Argument's type ({typeof(T).FullName})");

		if (param.IsOut)
			throw new ArgumentException($"{methodName}'s {param.Name} is an output parameter, which is unsupported in command methods");

		if (param.IsRetval)
			throw new ArgumentException($"{methodName}'s {param.Name} is a retval parameter, which is unsupported in command methods");

		// Determine whether the argument should be optional based on the nullability of the parameter's type
		if (underlying != null)
		{
			Type = underlying;
			IsOptional = true;
		}
		else if (_nullableContext.Create(param).WriteState == NullabilityState.Nullable)
		{
			IsOptional = true;
		}

		Name = param.Name;
		DefaultValue = (T?)param.DefaultValue;
	}
}
