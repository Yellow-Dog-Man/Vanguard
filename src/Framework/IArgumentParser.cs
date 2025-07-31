namespace Vanguard.Framework;

/// <summary>
/// Handles parsing of argument values for a type
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IArgumentParser<T>
{
	/// <summary>
	/// Parses an argument value into a useful form
	/// </summary>
	/// <param name="context"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public ValueTask<T> Parse(ICommandContext context, string value);
}
