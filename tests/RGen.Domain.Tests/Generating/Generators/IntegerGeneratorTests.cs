using System;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using RGen.Domain.Generating.Generators;
using Shouldly;


namespace RGen.Domain.Tests.Generating.Generators;


public class IntegerGeneratorTests
{
	private IntegerGenerator _sut = null!;
	private IntegerParameters _default;

	[SetUp]
	public void PreRun()
	{
		_sut = new IntegerGenerator();
		_default = new IntegerParameters(A.Dummy<int?>(), A.Dummy<ulong?>(), A.Dummy<ulong?>());
	}

#region Number of elements
	[TestCase(-1)]
	[TestCase(0)]
	public void Generate_should_throw_exception_if_NumberOfElements_is_out_of_range(int numberOfElements) =>
		Should.Throw<ArgumentOutOfRangeException>(() => _sut.Generate(numberOfElements, 1, _default));
#endregion Number of elements

#region Number of sets
	[TestCase(-1)]
	[TestCase(0)]
	public void Generate_should_throw_exception_if_NumberOfSets_is_out_of_range(int numberOfSets) =>
		Should.Throw<ArgumentOutOfRangeException>(() => _sut.Generate(1, numberOfSets, _default));
#endregion Number of sets

#region Length of element
	[TestCase(-1)]
	[TestCase(0)]
	[TestCase(20, Description = "long has 19 digits, excluding any sign")]
	public void Generate_should_throw_exception_if_Length_is_out_of_range(int length) =>
		Should.Throw<ArgumentOutOfRangeException>(() => _sut.Generate(1, 1, new IntegerParameters(length, A.Dummy<ulong?>(), A.Dummy<ulong?>())));

	[Test]
	public void Generate_with_length_specified_should_generate_number_containing_the_specified_number_of_digits()
	{
		const int numberOfDigits = 5;
		var result = _sut.Generate(1, 1, new IntegerParameters(numberOfDigits, A.Dummy<ulong?>(), A.Dummy<ulong?>())).ToArray();
		GeneratorUtils.PrintSets(result);
		result.Single().ToString().Length.ShouldBe(numberOfDigits);
	}

	[TestCase(1, 0UL, 9UL)]
	[TestCase(2, 10UL, 99UL)]
	[TestCase(3, 100UL, 999UL)]
	[TestCase(4, 1000UL, 9999UL)]
	[TestCase(5, 10000UL, 99999UL)]
	[TestCase(6, 100000UL, 999999UL)]
	[TestCase(7, 1000000UL, 9999999UL)]
	[TestCase(8, 10000000UL, 99999999UL)]
	[TestCase(9, 100000000UL, 999999999UL)]
	[TestCase(10, 1000000000UL, 9999999999UL)]
	[TestCase(11, 10000000000UL, 99999999999UL)]
	[TestCase(12, 100000000000UL, 999999999999UL)]
	[TestCase(13, 1000000000000UL, 9999999999999UL)]
	[TestCase(14, 10000000000000UL, 99999999999999UL)]
	[TestCase(15, 100000000000000UL, 999999999999999UL)]
	[TestCase(16, 1000000000000000UL, 9999999999999999UL)]
	[TestCase(17, 10000000000000000UL, 99999999999999999UL)]
	[TestCase(18, 100000000000000000UL, 999999999999999999UL)]
	[TestCase(19, 1000000000000000000UL, 9999999999999999999UL)]
	[TestCase(20, 10000000000000000000, ulong.MaxValue)]
	public void DetermineMinAndMax_should_return_boundary_values_if_requested_length_is_not_null(int requested, ulong expectedMin, ulong expectedMax)
	{
		var (resultMin, resultMax) = IntegerGenerator.DetermineMinAndMax(requested, null, null);
		resultMin.ShouldBe(expectedMin);
		resultMax.ShouldBe(expectedMax);
	}
#endregion Length of element

#region Clamping
	[Test]
	public void Generate_with_min_and_max_shall_throw_exception_if_min_is_greater_than_or_equal_to_max() =>
		Should.Throw<ArgumentOutOfRangeException>(() =>
			_sut.Generate(1, 1, new IntegerParameters(null, 1, 1)));

	[Test(Description = "Restrict only the lower boundary")]
	public void DetermineMinAndMax_shall_use_only_min_if_max_is_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(null, 1, null);
		minResult.ShouldBe(1UL);
		maxResult.ShouldBe(ulong.MaxValue);
	}

	[Test(Description = "Restrict only the upper boundary")]
	public void DetermineMinAndMax_shall_use_only_max_if_min_is_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(null, null, 1);
		minResult.ShouldBe(ulong.MinValue);
		maxResult.ShouldBe(1UL);
	}

	[Test(Description = "No restrictions on generated values")]
	public void DetermineMinAndMax_shall_return_null_if_length_and_min_and_max_are_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(null, null, null);
		minResult.ShouldBe(ulong.MinValue);
		maxResult.ShouldBe(ulong.MaxValue);
	}

	[Test(Description = "Calculate the boundaries based on number of digits")]
	public void DetermineMinAndMax_shall_return_length_based_boundaries_if_length_is_not_null_but_min_and_max_are_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(1, null, null);
		minResult.ShouldBe(0UL); // The smallest 1-digit number
		maxResult.ShouldBe(9UL); // The largest 1-digit number
	}

	[Test(Description = "Length is set to one digit, making valid values 0-9, but min is set to 2 so use that value instead")]
	public void DetermineMinAndMax_shall_return_min_if_length_is_not_null_but_min_is_also_not_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(1, 2, null);
		minResult.ShouldBe(2UL); // Not 0
		maxResult.ShouldBe(9UL); // Because length is specified
	}

	[Test(Description = "Length is set to one digit, making valid values 0-9, but max is set to 2 so use that value instead")]
	public void DetermineMinAndMax_shall_return_max_if_length_is_not_null_but_max_is_also_not_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(1, null, 2);
		minResult.ShouldBe(0UL); // Because length is specified
		maxResult.ShouldBe(2UL); // Not 9
	}

	[Test(Description = "Length is two digits, making valid values 10-99, but min is set to < 10")]
	public void DetermineMinAndMax_shall_throw_exception_if_length_is_specified_but_min_is_smaller() =>
		Should.Throw<InvalidOperationException>(() =>
			IntegerGenerator.DetermineMinAndMax(2, 9, null));

	[Test(Description = "Length is two digits, making valid values 10-99, but min is set to > 99")]
	public void DetermineMinAndMax_shall_throw_exception_if_length_is_specified_but_min_is_greater() =>
		Should.Throw<InvalidOperationException>(() =>
			IntegerGenerator.DetermineMinAndMax(2, 100, null));

	[Test(Description = "Length is two digits, making valid values 10-99, but min is set to > 99")]
	public void DetermineMinAndMax_shall_throw_exception_if_length_is_specified_but_max_is_greater() =>
		Should.Throw<InvalidOperationException>(() =>
			IntegerGenerator.DetermineMinAndMax(2, null, 100));

	[Test(Description = "Length is two digits, making valid values 10-99, but min is set to < 10")]
	public void DetermineMinAndMax_shall_throw_exception_if_length_is_specified_but_max_is_smaller() =>
		Should.Throw<InvalidOperationException>(() =>
			IntegerGenerator.DetermineMinAndMax(2, null, 9));

	[TestCase(null, null, null, ulong.MinValue, ulong.MaxValue)]
	[TestCase(1, null, null, 0UL, 9UL)]
	[TestCase(2, null, null, 10UL, 99UL)]
	[TestCase(2, 50UL, null, 50UL, 99UL)]
	[TestCase(2, null, 50UL, 10UL, 50UL)]
	[TestCase(2, 50UL, 75UL, 50UL, 75UL)]
	[TestCase(null, 127UL, null, 127UL, ulong.MaxValue, Description = "Only lower boundary")]
	[TestCase(null, null, 255UL, ulong.MinValue, 255UL, Description = "Only upper boundary")]
	[TestCase(null, 127UL, 255UL, 127UL, 255UL, Description = "Lower and upper boundary")]
	[TestCase(3, null, null, 100UL, 999UL, Description = "Only length")]
	[TestCase(3, 127UL, null, 127UL, 999UL, Description = "Length with lower boundary")]
	[TestCase(3, null, 255UL, 100UL, 255UL, Description = "Length with upper boundary")]
	[TestCase(3, 127UL, 255UL, 127UL, 255UL, Description = "Length with lower and upper boundary")]
	public void DetermineMinAndMax_cases_that_should_be_valid(int? length, ulong? min, ulong? max, ulong expectedMin, ulong expectedMax)
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(length, min, max);
		minResult.ShouldBe(expectedMin);
		maxResult.ShouldBe(expectedMax);
	}

	[TestCase(null, 1UL, 1UL)]
	[TestCase(null, 2UL, 1UL)]
	[TestCase(2, 9UL, null)]
	[TestCase(2, null, 100UL)]
	[TestCase(2, 9UL, 100UL)]
	public void DetermineMinAndMax_cases_that_should_throw_exceptions(int? length, ulong? min, ulong? max) =>
		Should.Throw<Exception>(() =>
			IntegerGenerator.DetermineMinAndMax(length, min, max));
#endregion Clamping

#region Generation
	[Test]
	public void Single_shall_generate_a_long_value() =>
		IntegerGenerator.Single(ulong.MinValue, ulong.MaxValue).ShouldBeOfType<ulong>();

	[Test]
	public void Single_shall_generate_a_Int32_if_max_is_less_than_or_equal_to_Int32_MaxValue() =>
		IntegerGenerator.Single(ulong.MinValue, int.MaxValue).ShouldBeLessThanOrEqualTo((ulong)int.MaxValue);
#endregion Generation

#region Known edge cases
	[Test]
	public void Single_shall_handle_known_edge_case_with_min_and_max() =>
		IntegerGenerator.Single(ulong.MinValue, ulong.MaxValue)
			.ShouldNotBe(9223372036854775808);

	[Test]
	public void Single_shall_handle_known_edge_case_with_sub_9() =>
		IntegerGenerator.Single(0, 5)
			.ShouldBeInRange(0UL, 5UL);
#endregion Known edge cases

	[Test]
	public void Generate_single_value_in_single_set_should_generate_a_single_value_in_a_single_set()
	{
		var result = _sut.Generate(1, 1, _default).ToArray();
		result.Length.ShouldBe(1);
	}

	[Test]
	public void Generate_single_value_multiple_sets_should_generate_single_values_in_multiple_sets()
	{
		var result = _sut.Generate(1, 2, _default).ToArray();
		result.Length.ShouldBe(2);
	}

	[Test]
	public void Generate_multiple_values_in_single_set_should_generate_multiple_values_in_a_single_set()
	{
		var result = _sut.Generate(2, 1, _default).ToArray();
		result.Length.ShouldBe(2 * 1);
	}

	[Test]
	public void Generate_multiple_values_in_multiple_sets_should_generate_multiple_values_in_multiple_sets()
	{
		var result = _sut.Generate(2, 3, _default).ToArray();
		result.Length.ShouldBe(2 * 3);
	}
}