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

	[TestCase(-1)]
	[TestCase(0)]
	public void Generate_should_throw_exception_if_NumberOfElements_is_out_of_range(int numberOfElements) =>
		Should.Throw<ArgumentOutOfRangeException>(() => _sut.Generate(numberOfElements, 1, A.Dummy<int?>()));

	[TestCase(-1)]
	[TestCase(0)]
	public void Generate_should_throw_exception_if_NumberOfSets_is_out_of_range(int numberOfSets) =>
		Should.Throw<ArgumentOutOfRangeException>(() => _sut.Generate(1, numberOfSets, A.Dummy<int?>()));

	[TestCase(-1)]
	[TestCase(0)]
	public void Generate_should_throw_exception_if_Length_is_out_of_range(int length) =>
		Should.Throw<ArgumentOutOfRangeException>(() => _sut.Generate(1, 1, length));

	[Test]
	public void Generate_single_value_in_single_set_should_generate_a_single_value_in_a_single_set()
	{
		var result = _sut.Generate(1, 1, A.Dummy<int?>());
		var resultSets = result.ValueSets.ToArray();
		resultSets.Length.ShouldBe(1);
		resultSets[0].Count().ShouldBe(1);
	}

	[Test]
	public void Generate_single_value_multiple_sets_should_generate_a_single_values_in_multiple_sets()
	{
		var result = _sut.Generate(1, 2, A.Dummy<int?>());
		var resultSets = result.ValueSets.ToArray();
		resultSets.Length.ShouldBe(2);
		resultSets.ShouldAllBe(s => s.Count() == 1);
	}

	[Test]
	public void Generate_multiple_values_in_single_set_should_generate_multiple_values_in_a_single_set()
	{
		var result = _sut.Generate(2, 1, A.Dummy<int?>());
		var resultSets = result.ValueSets.ToArray();
		resultSets.Length.ShouldBe(1);
		resultSets[0].Count().ShouldBe(2);
	}

	[Test]
	public void Generate_with_length_specified_should_generate_number_containing_the_specified_number_of_digits()
	{
		const int numberOfDigits = 5;
		var result = _sut.Generate(1, 1, numberOfDigits);
		result.ValueSets.First().First().ToString().Length.ShouldBe(numberOfDigits);
	}
}