using Vanguard.Framework;

namespace Vanguard.Dispatch;

/// <summary>
/// Default Vanguard registry
/// </summary>
public class Registry : IRegistry
{
	/// <summary>
	/// Registered commands mapped by all of their identifiers
	/// </summary>
	protected IDictionary<string, Command> _commands { get; init; } = new Dictionary<string, Command>(StringComparer.OrdinalIgnoreCase);

	/// <summary>
	/// Registered argument parsers mapped by their type
	/// </summary>
	protected IDictionary<Type, Type> _argParsers { get; init; } = new Dictionary<Type, Type>();

	public IEnumerable<Command> Commands => _commands.Values.Distinct();

	public IRegistry RegisterCommand(Command command)
	{
		_commands.Add(command.Name, command);

		foreach (var alias in command.Aliases)
			_commands.Add(alias, command);

		return this;
	}

	public IRegistry UnregisterCommand(Command command)
	{
		if (!_commands.ContainsKey(command.Name))
			throw new ArgumentOutOfRangeException($"Command {command.Name} isn't registered");

		_commands.Remove(command.Name);

		foreach (var alias in command.Aliases)
			_commands.Remove(alias);

		return this;
	}

	public Command? GetCommand(string id)
	{
		return _commands[id];
	}

	public bool TryGetCommand(string id, out Command? command)
	{
		return _commands.TryGetValue(id, out command);
	}

	public IRegistry RegisterArgumentParser<T, P>() where P : IArgumentParser<T>
	{
		_argParsers.Add(typeof(T), typeof(P));
		return this;
	}

	public IRegistry UnregisterArgumentParser<T>(out Type parser)
	{
		if (!_argParsers.ContainsKey(typeof(T)))
			throw new ArgumentOutOfRangeException($"No registered argument parser for type {typeof(T)}");

		_argParsers.Remove(typeof(T), out parser!);
		return this;
	}

	public Type? GetArgumentParser<T>()
	{
		return _argParsers[typeof(T)];
	}

	public bool TryGetArgumentParser<T>(out Type? parserType)
	{
		return _argParsers.TryGetValue(typeof(T), out parserType);
	}
}
