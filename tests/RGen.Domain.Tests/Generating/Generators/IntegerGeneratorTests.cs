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

	[Test]
	public void DetermineMinAndMaxBasedOnLength_should_return_null_if_requested_length_is_null()
	{
		var (resultMin, resultMax) = IntegerGenerator.DetermineMinAndMaxBasedOnLength(null);
		resultMin.ShouldBeNull();
		resultMax.ShouldBeNull();
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
	public void DetermineMinAndMaxBasedOnLength_should_return_boundary_values_if_requested_length_is_not_null(int requested, long? expectedMin, long? expectedMax)
	{
		var (resultMin, resultMax) = IntegerGenerator.DetermineMinAndMaxBasedOnLength(requested);
		resultMin.ShouldBe(expectedMin);
		resultMax.ShouldBe(expectedMax);
	}
#endregion Length of element

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

	[Test]
	public void Generate_with_both_length_and_min_shall_throw_exception_if_min_has_more_digits_than_length() =>
		Should.Throw<InvalidOperationException>(() => _sut.Generate(1, 1, 1, 10, A.Dummy<int>()));

	[Test]
	public void Generate_with_both_length_and_max_shall_throw_exception_if_max_has_more_digits_than_length() =>
		Should.Throw<InvalidOperationException>(() => _sut.Generate(1, 1, 1, A.Dummy<int>(), 10));

	[Test]
	public void Generate_with_both_length_and_min_and_max_shall_throw_exception_if_min_or_max_has_more_digits_than_length() =>
		Should.Throw<InvalidOperationException>(() => _sut.Generate(1, 1, 1, 10, 10));
}