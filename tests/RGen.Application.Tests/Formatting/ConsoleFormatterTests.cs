using FakeItEasy;
using NUnit.Framework;
using RGen.Application.Formatting;
using Shouldly;


namespace RGen.Application.Tests.Formatting;

public class ConsoleFormatterTests
{
	private ConsoleFormatter _sut;

#region Sample Data
	private static readonly int[][] SingleSetSingleValue =
		{
			new[]
				{
					1
				}
		};

	private static readonly int[][] SingleSetMultipleValues =
		{
			new[]
				{
					1,
					2
				}
		};

	private static readonly int[][] MultipleSetsSingleValue =
		{
			new[]
				{
					1
				},
			new[]
				{
					2
				}
		};

	private static readonly int[][] MultipleSetsMultipleValues =
		{
			new[]
				{
					1,
					2
				},
			new[]
				{
					3,
					4
				}
		};
#endregion Sample Data

	[SetUp]
	public void PreRun()
	{
		_sut = new ConsoleFormatter(A.Dummy<ConsoleFormatterOptions>());
	}

	[Test]
	public void Format_single_set_single_value_should_not_append_nor_suffix() =>
		_sut.Format(SingleSetSingleValue)
			.Dump()
			.ShouldBe("1");

	[Test]
	public void Format_single_set_multiple_values_should_not_append_nor_suffix() =>
		_sut.Format(SingleSetMultipleValues)
			.Dump()
			.ShouldBe("1\r\n2");

	[Test]
	public void Format_multiple_sets_single_value_should_not_append_nor_suffix() =>
		_sut.Format(MultipleSetsSingleValue)
			.Dump()
			.ShouldBe("1\r\n2");

	[Test]
	public void Format_multiple_sets_with_multiple_values_should_enclose_set() =>
		_sut.Format(MultipleSetsMultipleValues)
			.Dump()
			.ShouldBe("[1, 2]\r\n[3, 4]");

//TODO: Test coloring
}