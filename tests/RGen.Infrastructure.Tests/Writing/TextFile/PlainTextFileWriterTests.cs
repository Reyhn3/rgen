using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using RGen.Infrastructure.Writing.TextFile;
using Shouldly;


namespace RGen.Infrastructure.Tests.Writing.TextFile;

public class PlainTextFileWriterTests
{
	[Test]
	public void TryGetOrCreateFileName_should_expand_relative_paths()
	{
		var relative = Path.Join(Directory.GetCurrentDirectory(), "..");
		Console.WriteLine(relative);


		var result = PlainTextFileWriter.TryGetOrCreateFileName(new FileInfo(relative), out var actual);


		Console.WriteLine();
		Console.WriteLine(actual);
		result.ShouldBeTrue();
		actual!.ShouldNotContain("..");
	}

//TODO: Add logic for file path validation
	[Ignore("Temporarily ignored because file path validation logic is not in place yet")]
	[Test]
	public void TryGetOrCreateFileName_should_return_false_for_invalid_paths() =>
		PlainTextFileWriter.TryGetOrCreateFileName(new FileInfo(Path.GetInvalidPathChars().First().ToString()), out _)
			.ShouldBeFalse();

//TODO: Add logic for file name validation
	[Ignore("Temporarily ignored because file path validation logic is not in place yet")]
	[Test]
	public void TryGetOrCreateFileName_should_return_false_for_invalid_filenames() =>
		PlainTextFileWriter.TryGetOrCreateFileName(new FileInfo(Path.GetInvalidFileNameChars().First().ToString()), out _)
			.ShouldBeFalse();
}