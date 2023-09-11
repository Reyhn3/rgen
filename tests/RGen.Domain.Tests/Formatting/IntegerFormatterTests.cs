using System;
using FakeItEasy;
using NUnit.Framework;
using RGen.Domain.Formatting;
using RGen.Domain.Generating.Generators;
using Shouldly;


namespace RGen.Domain.Tests.Formatting;


public class IntegerFormatterTests
{
	[Test]
	public void FormatElement_should_return_plain_element_if_format_is_unrecognized() =>
		IntegerFormatter.FormatElement((IntegerFormat)(-1), 42).ShouldBe("42");

	[TestCase(IntegerFormat.Decimal, "7357")]
	[TestCase(IntegerFormat.Hexadecimal, "1cbd")]
	[TestCase(IntegerFormat.Binary, "0001110010111101")]
	public void FormatElement_should_return_the_number_correctly_formatted(IntegerFormat format, string expected) =>
		IntegerFormatter.FormatElement(format, 7357).ShouldBe(expected);
}