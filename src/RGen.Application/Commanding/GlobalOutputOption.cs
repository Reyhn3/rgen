using System.CommandLine;
using System.IO;


namespace RGen.Application.Commanding;

public static class GlobalOutputOption
{
	public static Option<FileInfo?> Create() => new(
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