using System;
using System.IO;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RGen.Infrastructure.Writing.TextFile;
using Shouldly;


namespace RGen.Infrastructure.Tests.Writing.TextFile;


public class TextFileWriterTests
{
	private TextFileWriter _sut = null!;

	[SetUp]
	public void PreRun()
	{
		_sut = new TextFileWriter(A.Dummy<ILogger<TextFileWriter>>(), A.Dummy<TextFileWriterOptions>());
	}

	[Test]
	public void TryGetOrCreateFileName_should_expand_relative_paths()
	{
		var relative = Path.Join(Directory.GetCurrentDirectory(), "..");
		Console.WriteLine(relative);


		var result = _sut.TryGetOrCreateFileName(new FileInfo(relative), out var actual);


		Console.WriteLine();
		Console.WriteLine(actual);
		result.ShouldBeTrue();
		actual!.ShouldNotContain("..");
	}

	[Test(Description = "A valid path that does not exist is OK because it will be created")]
	public void TryGetOrCreateFileName_should_return_true_if_the_path_is_valid_but_does_not_exist()
	{
		_sut.TryGetOrCreateFileName(new FileInfo(Path.Join(Path.GetTempPath(), "does", "not", "exist.txt")), out var result).ShouldBeTrue();
		Path.GetFileName(result).ShouldNotBeNullOrEmpty();
	}

	[Test(Description = "A path to an existing folder is OK because a random file will be generated inside it")]
	public void TryGetOrCreateFileName_should_return_true_if_the_path_is_an_existing_folder()
	{
		_sut.TryGetOrCreateFileName(new FileInfo(Path.Join(Path.GetTempPath())), out var result).ShouldBeTrue();
		Path.GetFileName(result).ShouldNotBeNullOrEmpty();
	}

	[Test(Description = "A path to an existing file is OK because the file will be overwritten")]
	public void TryGetOrCreateFileName_should_return_true_if_the_path_is_an_existing_file()
	{
		var input = new FileInfo(Path.GetTempFileName());
		_sut.TryGetOrCreateFileName(input, out var result).ShouldBeTrue();
		Path.GetFileName(result).ShouldBe(input.Name);
	}

	[Test(Description = "A path to a file that does not exist is OK because it will be created")]
	public void TryGetOrCreateFileName_should_return_true_if_the_path_is_a_file_that_does_not_exist()
	{
		const string filename = "does-not-exist-yet.txt";
		var input = Path.Join(Path.GetTempPath(), filename);

		_sut.TryGetOrCreateFileName(new FileInfo(input), out var result).ShouldBeTrue();
		Path.GetFileName(result).ShouldBe(filename);

		File.Delete(input);
	}

	[Test]
	public void IsDirectory_should_return_true_if_the_path_is_an_existing_directory() =>
		TextFileWriter.IsDirectory(Path.GetTempPath())
			.ShouldNotBeNull().ShouldBeTrue();

	[Test]
	public void IsDirectory_should_return_false_if_the_path_is_an_existing_file() =>
		TextFileWriter.IsDirectory(Path.GetTempFileName())
			.ShouldNotBeNull().ShouldBeFalse();

	[Test]
	public void IsDirectory_should_return_null_if_the_path_does_not_exist() =>
		TextFileWriter.IsDirectory(@"C:\Does\Not\Exist")
			.ShouldBeNull();
}