using System.CommandLine;


namespace RGen.Application;

public static class GlobalSilentOption
{
	public const string SilentOption = "--silent";

	public static Option<bool> Create() => new(
			new[]
				{
					SilentOption
				},
			() => false,
			"Don't show the beautiful splash graphics")
			{
				IsRequired = false
			};
}