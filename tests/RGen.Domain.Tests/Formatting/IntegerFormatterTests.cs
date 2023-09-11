using NUnit.Framework;
using RGen.Domain.Formatting;
using RGen.Domain.Generating.Generators;
using Shouldly;


namespace RGen.Domain.Tests.Formatting;


public class IntegerFormatterTests
{
	[Test]
	public void FormatElement_should_return_plain_element_if_format_is_unrecognized() =>
		IntegerFormatter.FormatElement((IntegerBase)(-1), 42).ShouldBe("42");

	[TestCase(IntegerBase.Decimal, "7357")]
	[TestCase(IntegerBase.Hexadecimal, "1cbd")]
	[TestCase(IntegerBase.Binary, "0001110010111101")]
	public void FormatElement_should_return_the_number_correctly_formatted(IntegerBase format, string expected) =>
		IntegerFormatter.FormatElement(format, 7357).ShouldBe(expected);
}