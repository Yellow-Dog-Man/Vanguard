namespace Vanguard;

using ArgumentParser = Func<string, ICommandContext, Task<object>>;

/// <summary>
/// Deals with registration and retrieval of all Vanguard types that are necessary to dispatch commands
/// </summary>
public interface IRegistry
{
	/// <summary>
	/// Gets an enumerable container of all registered commands
	/// </summary>
	public IEnumerable<Command> Commands { get; }

	// TODO: no need to track controllers, replace with concrete method - perhaps RegisterAllCommands([namespace])?
	// /// <summary>
	// /// Registers a single command controller type. Optionally automatically registers all methods in it that are marked
	// /// with a <see cref="CommandAttribute">CommandAttribute</see>.
	// /// </summary>
	// /// <param name="controller"></param>
	// /// <param name="registerCommands"></param>
	// /// <returns></returns>
	// public IRegistry RegisterController(Type controller, bool registerCommands = true);

	// /// <summary>
	// /// Unregisters a single command controller type. Optionally automatically unregisters all methods in it that are
	// /// marked with a <see cref="CommandAttribute">CommandAttribute</see>.
	// /// </summary>
	// /// <param name="controller"></param>
	// /// <param name="unregisterCommands"></param>
	// /// <returns></returns>
	// public IRegistry UnregisterController(Type controller, bool unregisterCommands = true);

	/// <summary>
	/// Registers a command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	public IRegistry RegisterCommand(Command command);

	/// <summary>
	/// Unregisters a command
	/// </summary>
	/// <param name="command"></param>
	/// <returns></returns>
	public IRegistry UnregisterCommand(Command command);

	/// <summary>
	/// Unregisters a command by any of its identifiers
	/// </summary>
	/// <param name="id"></param>
	/// <param name="command">Command that was unregistered</param>
	/// <returns></returns>
	public IRegistry UnregisterCommand(string id, out Command command)
	{
		command = ExpectCommand(id);
		return UnregisterCommand(command);
	}

	/// <summary>
	/// Unregisters a command by any of its identifiers
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public IRegistry UnregisterCommand(string id)
	{
		var command = ExpectCommand(id);
		return UnregisterCommand(command);
	}

	/// <summary>
	/// Retrieves a command by any of its identifiers
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Command? GetCommand(string id);

	/// <summary>
	/// Retrieves a command by any of its identifiers
	/// </summary>
	/// <param name="id"></param>
	/// <param name="command">Retrieved command</param>
	/// <returns>Whether the registry has a command with the provided identifier</returns>
	public bool TryGetCommand(string id, out Command? command);

	/// <summary>
	/// Retrieves a command by any of its identifiers, throwing if one cannot be found
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentOutOfRangeException">When no command is registered with the provided identifier</exception>
	public Command ExpectCommand(string id)
	{
		if (!TryGetCommand(id, out var command))
			throw new ArgumentOutOfRangeException($"No registered command with \"{id}\" identifier");

		return command!;
	}

	/// <summary>
	/// Registers an argument parser for a type
	/// </summary>
	/// <param name="type"></param>
	/// <param name="parser"></param>
	/// <returns></returns>
	public IRegistry RegisterArgumentParser(Type type, ArgumentParser parser);

	/// <summary>
	/// Unregisters the argument parser for a type
	/// </summary>
	/// <param name="type"></param>
	/// <param name="parser">Argument parser that was unregistered</param>
	/// <returns></returns>
	public IRegistry UnregisterArgumentParser(Type type, out ArgumentParser parser)
	{
		parser = ExpectArgumentParser(type);
		return UnregisterArgumentParser(type);
	}

	/// <summary>
	/// Unregisters the argument parser for a type
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public IRegistry UnregisterArgumentParser(Type type);

	/// <summary>
	/// Retrieves the argument parser for a type
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public ArgumentParser? GetArgumentParser(Type type);

	/// <summary>
	/// Retrieves the argument parser for a type
	/// </summary>
	/// <param name="type"></param>
	/// <param name="parser">Retrieved parser</param>
	/// <returns>Whether the registry has an argument parser for the provided type</returns>
	public bool TryGetArgumentParser(Type type, out ArgumentParser? parser);

	/// <summary>
	/// Retrieves the argument parser for a type, throwing if one isn't registered
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentOutOfRangeException">When no argument parser is registered for the provided type</exception>
	public ArgumentParser ExpectArgumentParser(Type type)
	{
		if (!TryGetArgumentParser(type, out var parser))
			throw new ArgumentOutOfRangeException($"No registered argument parser for type {type}");

		return parser!;
	}
}
