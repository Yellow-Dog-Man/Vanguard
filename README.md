# Vanguard

Vanguard is a WIP command framework that will soon be used to build commands for the in-game Resonite bot and the headless client.

## Design

It's very much in flux!
Nothing is set in stone yet since the framework is super WIP.

The idea is to be very familiar to those who work with ASP.NET controllers, so the goal is to make creation of commands look something like this:

```cs
using Vanguard;

namespace SomeService.Example;

public class ExampleCommands(IUserService userService, ISpookyService spookyService) : CommandController
{
	// Example usage: /ping
	[Command]
	public async Task Ping()
	{
		await Respond("Pong!");
	}

	// Example usage: /dangerous, /breakthings, or /explode
	[Command(Aliases = ["breakthings", "explode"])]
	[Permission(PermissionLevel.SuperAdmin)]
	public async Task Dangerous()
	{
		await spookyService.BreakAllTheThings();
		await Respond("Congratulations, everything has exploded!");
	}

	// Example usage: /addtag <user> <tag>
	[Command]
	[Permission(PermissionLevel.Admin)]
	public async Task AddTag(User user, string tag)
	{
		await userService.AddUserTag(user, tag);
		await Respond($"Added {tag} tag to {user.Name}.");
	}

	// Example usage: /rename [user] <new name>
	// maybe better using overloads vs. nullable params?
	[Command(Name = "rename")]
	public async Task RenameUser(User? user, string name)
	{
		// Only allow the command user to rename somebody other than themself if they're an admin
		if (user != null && user.Id != User.Id && !await HasPermission(PermissionLevel.Admin))
		{
			await Respond("You may only rename yourself.");
			return;
		}

		var userToRename = user ?? User;
		var oldName = user.Name;

		await userService.RenameUser(user, name);

		await Respond($"Renamed {oldName} to {name}.");
	}
}
```

## Planned features

- Automatic argument parsing, with support for optional arguments
- Permissions checking abstractions
- Customizability! Almost every component should be replaceable/tweakable
- Dependency injection
- Automatic (or manual) controller & command registration
