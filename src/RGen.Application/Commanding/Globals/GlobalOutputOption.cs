using System.CommandLine;
using System.IO;


namespace RGen.Application.Commanding.Globals;

public static class GlobalOutputOption
{
	public static Option<FileInfo?> Output = new(
			new[]
				{
					"-o",
					"--output"
				},
			() => null,
			"Write values to file")
			{
				IsRequired = false
			};
}