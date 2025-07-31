using Vanguard.Framework;

namespace Vanguard.Dispatch;

/// <summary>
/// Deals with registration and retrieval of all Vanguard types that are necessary to dispatch commands
/// </summary>
public interface IRegistry
{
	/// <summary>
	/// Gets an enumerable container of all registered commands
	/// </summary>
	public IEnumerable<Command> Commands { get; }

	// /// <summary>
	// /// Registers a single command controller. Optionally automatically registers all methods in it that are marked
	// /// with a <see cref="CommandAttribute">CommandAttribute</see>.
	// /// </summary>
	// /// <param name="controller"></param>
	// /// <param name="registerCommands"></param>
	// /// <returns></returns>
	// public IRegistry RegisterController(Type controller, bool registerCommands = true);

	// /// <summary>
	// /// Unregisters a single command controller. Optionally automatically unregisters all methods in it that are
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
		return UnregisterCommand(id, out _);
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
	/// Registers the argument parser to use for a type
	/// </summary>
	/// <typeparam name="T">Type the parser handles</typeparam>
	/// <typeparam name="P">Argument parser</typeparam>
	/// <param name="parserType"></param>
	/// <returns></returns>
	public IRegistry RegisterArgumentParser<T, P>() where P : IArgumentParser<T>;

	/// <summary>
	/// Unregisters the argument parser for a type
	/// </summary>
	/// <param name="parserType">Argument parser that was unregistered</param>
	/// <typeparam name="T">Type the parser handles</typeparam>
	/// <returns></returns>
	public IRegistry UnregisterArgumentParser<T>(out Type parserType);

	/// <summary>
	/// Unregisters the argument parser for a type
	/// </summary>
	/// <typeparam name="T">Type the parser handles</typeparam>
	/// <returns></returns>
	public IRegistry UnregisterArgumentParser<T>()
	{
		return UnregisterArgumentParser<T>(out _);
	}

	/// <summary>
	/// Retrieves the argument parser for a type
	/// </summary>
	/// <typeparam name="T">Type the parser handles</typeparam>
	/// <returns></returns>
	public Type? GetArgumentParser<T>();

	/// <summary>
	/// Retrieves the argument parser for a type
	/// </summary>
	/// <typeparam name="T">Type the parser handles</typeparam>
	/// <param name="parserType">Retrieved parser</param>
	/// <returns>Whether the registry has an argument parser for the provided type</returns>
	public bool TryGetArgumentParser<T>(out Type? parserType);

	/// <summary>
	/// Retrieves the argument parser for a type, throwing if one isn't registered
	/// </summary>
	/// <typeparam name="T">Type the parser handles</typeparam>
	/// <returns></returns>
	/// <exception cref="ArgumentOutOfRangeException">When no argument parser is registered for the provided type</exception>
	public Type ExpectArgumentParser<T>()
	{
		if (!TryGetArgumentParser<T>(out var parser))
			throw new ArgumentOutOfRangeException($"No registered argument parser for type {typeof(T)}");

		return parser!;
	}
}
