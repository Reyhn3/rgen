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

	[TestCase(IntegerBase.Decimal, 7357, "7357")]
	[TestCase(IntegerBase.Hexadecimal, 7357, "1cbd")]
	[TestCase(IntegerBase.Binary, 7357, "0001110010111101")]
	[TestCase(IntegerBase.Binary, 1, "00000001", Description = "Binaries should be padded to the smallest amount of bytes")]
	[TestCase(IntegerBase.Binary, 131071, "000000011111111111111111", Description = "Binaries should be padded to the smallest amount of bytes")]
	[TestCase(IntegerBase.Binary, 33554431, "00000001111111111111111111111111", Description = "Binaries should be padded to the smallest amount of bytes")]
	public void FormatElement_should_return_the_number_correctly_formatted(IntegerBase format, long value, string expected) =>
		IntegerFormatter.FormatElement(format, value).ShouldBe(expected);
}