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

//TODO: Add tests to assert a reasonable statistically distributed random confidence

	[Test]
	public void Generate_shall_produce_enumerable_values_each_time_the_enumerable_is_iterated()
	{
		var result = _sut.Generate(1, 1, null, null, null);

		// The sequence should create new random values each time it is iterated
		var firstMaterialization = result.Single();
		var secondMaterialization = result.Single();
		firstMaterialization.ShouldNotBe(secondMaterialization);
	}

	[Test]
	public void Generate_shall_produce_enumerable_values_that_can_be_materialized()
	{
		var result = _sut.Generate(1, 1, null, null, null).ToArray();

		// The sequence should not be reiterated
		var firstMaterialization = result.Single();
		var secondMaterialization = result.Single();
		firstMaterialization.ShouldBe(secondMaterialization);
	}

	[Test]
	public void Generate_shall_generate_a_single_value()
	{
		var result = _sut.Generate(1, 1, null, null, null).ToArray();
		GeneratorUtils.PrintSets(result);
		result.Length.ShouldBe(1);
	}

	[Test]
	public void Generate_shall_generate_a_stream_with_the_length_of_the_sets_multiplied_with_set_length()
	{
		var result = _sut.Generate(1, 2, null, null, null).ToArray();
		GeneratorUtils.PrintSets(result);
		result.Length.ShouldBe(1 * 2);
	}

	[Test]
	public void Generate_shall_generate_many_values()
	{
		var result = _sut.Generate(100, 1, null, null, null).ToArray();
		GeneratorUtils.PrintSets(result);
		result.Length.ShouldBe(100 * 1);
	}

	[Test]
	public void Generate_shall_generate_very_many_values()
	{
		var result = _sut.Generate(100, 100, null, null, null).ToArray();
		GeneratorUtils.PrintSets(result);
		result.Length.ShouldBe(100 * 100);
	}
}