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

	[SetUp]
	public void PreRun()
	{
		_sut = new IntegerGenerator();
	}

#region Number of elements
	[TestCase(-1)]
	[TestCase(0)]
	public void Generate_should_throw_exception_if_NumberOfElements_is_out_of_range(int numberOfElements) =>
		Should.Throw<ArgumentOutOfRangeException>(() => _sut.Generate(numberOfElements, 1, A.Dummy<int?>(), A.Dummy<int?>(), A.Dummy<int?>()));
#endregion Number of elements

#region Number of sets
	[TestCase(-1)]
	[TestCase(0)]
	public void Generate_should_throw_exception_if_NumberOfSets_is_out_of_range(int numberOfSets) =>
		Should.Throw<ArgumentOutOfRangeException>(() => _sut.Generate(1, numberOfSets, A.Dummy<int?>(), A.Dummy<int?>(), A.Dummy<int?>()));
#endregion Number of sets

#region Length of element
	[TestCase(-1)]
	[TestCase(0)]
	[TestCase(20, Description = "long has 19 digits, excluding any sign")]
	public void Generate_should_throw_exception_if_Length_is_out_of_range(int length) =>
		Should.Throw<ArgumentOutOfRangeException>(() => _sut.Generate(1, 1, length, A.Dummy<int?>(), A.Dummy<int?>()));

	[Test]
	public void Generate_with_length_specified_should_generate_number_containing_the_specified_number_of_digits()
	{
		const int numberOfDigits = 5;
		var result = _sut.Generate(1, 1, numberOfDigits, A.Dummy<int?>(), A.Dummy<int?>());
		GeneratorUtils.PrintSets(result);
		result.ValueSets.First().First().ToString()!.Length.ShouldBe(numberOfDigits);
	}

	[TestCase(1, 0, 9)]
	[TestCase(2, 10, 99)]
	[TestCase(3, 100, 999)]
	[TestCase(4, 1000, 9999)]
	[TestCase(5, 10000, 99999)]
	[TestCase(6, 100000, 999999)]
	[TestCase(7, 1000000, 9999999)]
	[TestCase(8, 10000000, 99999999)]
	[TestCase(9, 100000000, 999999999)]
	[TestCase(10, 1000000000, 9999999999)]
	[TestCase(11, 10000000000, 99999999999)]
	[TestCase(12, 100000000000, 999999999999)]
	[TestCase(13, 1000000000000, 9999999999999)]
	[TestCase(14, 10000000000000, 99999999999999)]
	[TestCase(15, 100000000000000, 999999999999999)]
	[TestCase(16, 1000000000000000, 9999999999999999)]
	[TestCase(17, 10000000000000000, 99999999999999999)]
	[TestCase(18, 100000000000000000, 999999999999999999)]
	[TestCase(19, 1000000000000000000, long.MaxValue)]
	public void DetermineMinAndMax_should_return_boundary_values_if_requested_length_is_not_null(int requested, long? expectedMin, long? expectedMax)
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
			_sut.Generate(1, 1, null, 1, 1));

	[Test(Description = "Restrict only the lower boundary")]
	public void DetermineMinAndMax_shall_use_only_min_if_max_is_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(null, 1, null);
		minResult.ShouldBe(1);
		maxResult.ShouldBeNull();
	}

	[Test(Description = "Restrict only the upper boundary")]
	public void DetermineMinAndMax_shall_use_only_max_if_min_is_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(null, null, 1);
		minResult.ShouldBeNull();
		maxResult.ShouldBe(1);
	}

	[Test(Description = "No restrictions on generated values")]
	public void DetermineMinAndMax_shall_return_null_if_length_and_min_and_max_are_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(null, null, null);
		minResult.ShouldBeNull();
		maxResult.ShouldBeNull();
	}

	[Test(Description = "Calculate the boundaries based on number of digits")]
	public void DetermineMinAndMax_shall_return_length_based_boundaries_if_length_is_not_null_but_min_and_max_are_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(1, null, null);
		minResult.ShouldBe(0); // The smallest 1-digit number
		maxResult.ShouldBe(9); // The largest 1-digit number
	}

	[Test(Description = "Length is set to one digit, making valid values 0-9, but min is set to 2 so use that value instead")]
	public void DetermineMinAndMax_shall_return_min_if_length_is_not_null_but_min_is_also_not_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(1, 2, null);
		minResult.ShouldBe(2); // Not 0
		maxResult.ShouldBe(9); // Because length is specified
	}

	[Test(Description = "Length is set to one digit, making valid values 0-9, but max is set to 2 so use that value instead")]
	public void DetermineMinAndMax_shall_return_max_if_length_is_not_null_but_max_is_also_not_null()
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(1, null, 2);
		minResult.ShouldBe(0); // Because length is specified
		maxResult.ShouldBe(2); // Not 9
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

	[TestCase(null, null, null, null, null)]
	[TestCase(1, null, null, 0, 9)]
	[TestCase(2, null, null, 10, 99)]
	[TestCase(2, 50, null, 50, 99)]
	[TestCase(2, null, 50, 10, 50)]
	[TestCase(2, 50, 75, 50, 75)]
	[TestCase(null, 127, null, 127, null, Description = "Only lower boundary")]
	[TestCase(null, null, 255, null, 255, Description = "Only upper boundary")]
	[TestCase(null, 127, 255, 127, 255, Description = "Lower and upper boundary")]
	[TestCase(3, null, null, 100, 999, Description = "Only length")]
	[TestCase(3, 127, null, 127, 999, Description = "Length with lower boundary")]
	[TestCase(3, null, 255, 100, 255, Description = "Length with upper boundary")]
	[TestCase(3, 127, 255, 127, 255, Description = "Length with lower and upper boundary")]
	public void DetermineMinAndMax_cases_that_should_be_valid(int? length, long? min, long? max, long? expectedMin, long? expectedMax)
	{
		var (minResult, maxResult) = IntegerGenerator.DetermineMinAndMax(length, min, max);
		minResult.ShouldBe(expectedMin);
		maxResult.ShouldBe(expectedMax);
	}

	[TestCase(null, 1, 1)]
	[TestCase(null, 2, 1)]
	[TestCase(2, 9, null)]
	[TestCase(2, null, 100)]
	[TestCase(2, 9, 100)]
	public void DetermineMinAndMax_cases_that_should_throw_exceptions(int? length, long? min, long? max) =>
		Should.Throw<Exception>(() =>
			IntegerGenerator.DetermineMinAndMax(length, min, max));
#endregion Clamping

#region Generation
	[Test]
	public void Single_shall_generate_a_long_value() =>
		IntegerGenerator.Single(null, null).ShouldBeOfType<long>();
	
	[Test]
	public void Single_shall_generate_a_Int32_if_max_is_less_than_or_equal_to_Int32_MaxValue() =>
		IntegerGenerator.Single(null, int.MaxValue).ShouldBeLessThanOrEqualTo(int.MaxValue);

	[Test]
	public void Single_shall_not_retry_forever_if_the_clamped_value_is_out_of_range() =>
		Should.Throw<Exception>(() => IntegerGenerator.Single(long.MaxValue - 1, null));
#endregion Generation

	[Test]
	public void Generate_single_value_in_single_set_should_generate_a_single_value_in_a_single_set()
	{
		var result = _sut.Generate(1, 1, A.Dummy<int?>(), A.Dummy<int?>(), A.Dummy<int?>());
		var resultSets = result.ValueSets.ToArray();
		resultSets.Length.ShouldBe(1);
		resultSets[0].Count().ShouldBe(1);
	}

	[Test]
	public void Generate_single_value_multiple_sets_should_generate_single_values_in_multiple_sets()
	{
		var result = _sut.Generate(1, 2, A.Dummy<int?>(), A.Dummy<int?>(), A.Dummy<int?>());
		var resultSets = result.ValueSets.ToArray();
		resultSets.Length.ShouldBe(2);
		resultSets.ShouldAllBe(s => s.Count() == 1);
	}

	[Test]
	public void Generate_multiple_values_in_single_set_should_generate_multiple_values_in_a_single_set()
	{
		var result = _sut.Generate(2, 1, A.Dummy<int?>(), A.Dummy<int?>(), A.Dummy<int?>());
		var resultSets = result.ValueSets.ToArray();
		resultSets.Length.ShouldBe(1);
		resultSets[0].Count().ShouldBe(2);
	}

	[Test]
	public void Generate_multiple_values_in_multiple_sets_should_generate_multiple_values_in_multiple_sets()
	{
		var result = _sut.Generate(2, 3, A.Dummy<int?>(), A.Dummy<int?>(), A.Dummy<int?>());
		var resultSets = result.ValueSets.ToArray();
		resultSets.Length.ShouldBe(3);
		resultSets.ShouldAllBe(s => s.Count() == 2);
	}
}