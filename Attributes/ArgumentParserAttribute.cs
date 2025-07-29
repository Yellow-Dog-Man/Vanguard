namespace Vanguard;

/// <summary>
/// Marks a class for registration as an argument parser
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ArgumentParserAttribute : Attribute { }
