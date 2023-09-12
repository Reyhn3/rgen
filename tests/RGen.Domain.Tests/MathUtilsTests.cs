using NUnit.Framework;
using Shouldly;


namespace RGen.Domain.Tests;


public class MathUtilsTests
{
	[Test]
	public void CountNumberOfDecimalDigits_should_return_1_for_value_0() =>
		MathUtils.CountNumberOfDecimalDigits(0).ShouldBe(1);

	[TestCase(1, 1UL)]
	[TestCase(1, 9UL)]
	[TestCase(2, 10UL)]
	[TestCase(2, 99UL)]
	[TestCase(3, 100UL)]
	[TestCase(3, 999UL)]
	[TestCase(4, 1000UL)]
	[TestCase(4, 9999UL)]
	[TestCase(5, 10000UL)]
	[TestCase(5, 99999UL)]
	[TestCase(6, 100000UL)]
	[TestCase(6, 999999UL)]
	[TestCase(7, 1000000UL)]
	[TestCase(7, 9999999UL)]
	[TestCase(8, 10000000UL)]
	[TestCase(8, 99999999UL)]
	[TestCase(9, 100000000UL)]
	[TestCase(9, 999999999UL)]
	[TestCase(10, 1000000000UL)]
	[TestCase(10, (ulong)int.MaxValue)]
	public void CountNumberOfDecimalDigits_for_int_should_return_the_correct_number_of_decimal_digits_for_positive_values(int expected, ulong value) =>
		MathUtils.CountNumberOfDecimalDigits(value).ShouldBe(expected);

	[TestCase(1, 1UL)]
	[TestCase(1, 9UL)]
	[TestCase(2, 10UL)]
	[TestCase(2, 99UL)]
	[TestCase(3, 100UL)]
	[TestCase(3, 999UL)]
	[TestCase(4, 1000UL)]
	[TestCase(4, 9999UL)]
	[TestCase(5, 10000UL)]
	[TestCase(5, 99999UL)]
	[TestCase(6, 100000UL)]
	[TestCase(6, 999999UL)]
	[TestCase(7, 1000000UL)]
	[TestCase(7, 9999999UL)]
	[TestCase(8, 10000000UL)]
	[TestCase(8, 99999999UL)]
	[TestCase(9, 100000000UL)]
	[TestCase(9, 999999999UL)]
	[TestCase(10, 1000000000UL)]
	[TestCase(10, 9999999999UL)]
	[TestCase(11, 10000000000UL)]
	[TestCase(11, 99999999999UL)]
	[TestCase(12, 100000000000UL)]
	[TestCase(12, 999999999999UL)]
	[TestCase(13, 1000000000000UL)]
	[TestCase(13, 9999999999999UL)]
	[TestCase(14, 10000000000000UL)]
	[TestCase(14, 99999999999999UL)]
	[TestCase(15, 100000000000000UL)]
	[TestCase(15, 999999999999999UL)]
	[TestCase(16, 1000000000000000UL)]
	[TestCase(16, 9999999999999999UL)]
	[TestCase(17, 10000000000000000UL)]
	[TestCase(17, 99999999999999999UL)]
	[TestCase(18, 100000000000000000UL)]
	[TestCase(18, 999999999999999999UL)]
	[TestCase(19, 1000000000000000000UL)]
	[TestCase(20, ulong.MaxValue)]
	public void CountNumberOfDecimalDigits_for_long_should_return_the_correct_number_of_decimal_digits_for_positive_values(int expected, ulong value) =>
		MathUtils.CountNumberOfDecimalDigits(value).ShouldBe(expected);
}