using System.Linq;
using NUnit.Framework;
using RGen.Domain.Generating.Generators;
using Shouldly;


namespace RGen.Domain.Tests.Generating.Generators;

public class IntegerGeneratorIntegrationTests
{
	private IntegerGenerator _sut;

	[SetUp]
	public void PreRun()
	{
		_sut = new IntegerGenerator();
	}

	[Test]
	public void IntegerGenerator_shall_generate_a_single_value_in_a_single_set()
	{
		var result = _sut.Generate(1, 1, null, null, null);
		GeneratorUtils.PrintSets(result);

		var resultArray = result.ValueSets.ToArray();
		resultArray.Length.ShouldBe(1);
		var resultSet = resultArray[0].ToArray();
		resultSet.Length.ShouldBe(1);
	}

	[Test]
	public void IntegerGenerator_shall_generate_a_single_value_in_every_set()
	{
		var result = _sut.Generate(1, 2, null, null, null);
		GeneratorUtils.PrintSets(result);

		var resultArray = result.ValueSets.ToArray();
		resultArray.Length.ShouldBe(2);
		var resultSet0 = resultArray[0].ToArray();
		resultSet0.Length.ShouldBe(1);
		var resultSet1 = resultArray[1].ToArray();
		resultSet1.Length.ShouldBe(1);
	}

	[Test]
	public void IntegerGenerator_shall_generate_many_values_in_a_single_set()
	{
		var result = _sut.Generate(100, 1, null, null, null);
		GeneratorUtils.PrintSets(result);

		var resultArray = result.ValueSets.ToArray();
		resultArray.Length.ShouldBe(1);
		var resultSet = resultArray[0].ToArray();
		resultSet.Length.ShouldBe(100);
	}

	[Test]
	public void IntegerGenerator_shall_generate_many_values_in_many_sets()
	{
		var result = _sut.Generate(100, 100, null, null, null);
		GeneratorUtils.PrintSets(result);

		var resultArray = result.ValueSets.ToArray();
		resultArray.Length.ShouldBe(100);
		resultArray.ShouldAllBe(s => s.Count() == 100);
	}
}