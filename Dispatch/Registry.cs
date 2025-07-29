namespace Vanguard;

using ArgumentParser = Func<string, ICommandContext, Task<object>>;

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
	protected IDictionary<Type, ArgumentParser> _argParsers { get; init; } = new Dictionary<Type, ArgumentParser>();

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

	public IRegistry RegisterArgumentParser(Type type, ArgumentParser parser)
	{
		_argParsers.Add(type, parser);
		return this;
	}

	public IRegistry UnregisterArgumentParser(Type type)
	{
		_argParsers.Remove(type);
		return this;
	}

	public ArgumentParser? GetArgumentParser(Type type)
	{
		return _argParsers[type];
	}

	public bool TryGetArgumentParser(Type type, out ArgumentParser? parser)
	{
		return _argParsers.TryGetValue(type, out parser);
	}
}
