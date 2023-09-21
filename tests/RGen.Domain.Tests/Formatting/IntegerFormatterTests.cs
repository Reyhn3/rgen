using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RGen.Domain.Formatting;
using RGen.Domain.Generating.Generators;
using Shouldly;


namespace RGen.Domain.Tests.Formatting;


public class IntegerFormatterTests
{
	private IntegerFormatter _sut = null!;

	[SetUp]
	public void PreRun()
	{
		_sut = new IntegerFormatter(A.Dummy<ILogger<IntegerFormatter>>(), A.Dummy<IntegerFormatterOptions>());
	}

	[Test]
	public void FormatElement_should_return_plain_element_if_format_is_unrecognized() =>
		_sut.FormatElement((IntegerBase)(-1), 42).ShouldBe("42");

	[TestCase(7357UL, "7357")]
	public void FormatElement_should_correctly_format_decimal_numbers(ulong value, string expected) =>
		_sut.FormatElement(IntegerBase.Decimal, value).ShouldBe(expected);

	[TestCase(7357UL, "1cbd")]
	public void FormatElement_should_correctly_format_hexadecimal_numbers(ulong value, string expected) =>
		_sut.FormatElement(IntegerBase.Hexadecimal, value).ShouldBe(expected);

	[TestCase(7357UL, "0001110010111101")]
	[TestCase(1UL, "00000001")]
	[TestCase(511UL, "0000000111111111")]
	[TestCase(131071UL, "000000011111111111111111")]
	[TestCase(33554431UL, "00000001111111111111111111111111")]
	public void FormatElement_should_correctly_format_binary_numbers(ulong value, string expected) =>
		_sut.FormatElement(IntegerBase.Binary, value).ShouldBe(expected);

	[Test]
	public void FormatAsBitString_shall_use_the_next_largest_multiple_of_8() =>
		IntegerFormatter.FormatAsBitString(7357).ShouldBe("0001110010111101");
}