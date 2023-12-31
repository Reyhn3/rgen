﻿using System;
using System.IO;
using System.Linq;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RGen.Infrastructure.Writing.TextFile;
using Shouldly;


namespace RGen.Infrastructure.Tests.Writing.TextFile;

public class PlainTextFileWriterTests
{
	private PlainTextFileWriter _sut = null!;

	[SetUp]
	public void PreRun()
	{
		_sut = new PlainTextFileWriter(A.Dummy<ILogger<PlainTextFileWriter>>(), A.Dummy<PlainTextFileWriterOptions>());
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

//TODO: Add logic for file path validation
	[Ignore("Temporarily ignored because file path validation logic is not in place yet")]
	[Test]
	public void TryGetOrCreateFileName_should_return_false_for_invalid_paths() =>
		_sut.TryGetOrCreateFileName(new FileInfo(Path.GetInvalidPathChars().First().ToString()), out _)
			.ShouldBeFalse();

//TODO: Add logic for file name validation
	[Ignore("Temporarily ignored because file path validation logic is not in place yet")]
	[Test]
	public void TryGetOrCreateFileName_should_return_false_for_invalid_filenames() =>
		_sut.TryGetOrCreateFileName(new FileInfo(Path.GetInvalidFileNameChars().First().ToString()), out _)
			.ShouldBeFalse();
}