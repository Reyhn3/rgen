using System.CommandLine;


namespace RGen.Application.Commanding.Globals;

public static class GlobalColorOption
{
	public static Option<bool> Color = new(
			new[]
				{
					"--no-color"
				},
			() => false,
			"Disable console output colors")
			{
				IsRequired = false
			};
}