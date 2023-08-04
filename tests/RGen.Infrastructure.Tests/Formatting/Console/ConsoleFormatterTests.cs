using FakeItEasy;
using NUnit.Framework;
using RGen.Domain.Formatting;
using RGen.Domain.Generating;
using RGen.Infrastructure.Formatting.Console;
using Shouldly;


namespace RGen.Infrastructure.Tests.Formatting.Console;

public class ConsoleFormatterTests
{
	private ConsoleFormatter _sut = null!;

#region Sample Data
	// @formatter:off
	private static readonly IRandomValues<int> SingleSetSingleValue = RandomValues.Create(new[]
		{
			new[] { 1 }
		});

	private static readonly IRandomValues<int> SingleSetMultipleValues = RandomValues.Create(new[]
		{
			new[] { 1, 2 }
		});

	private static readonly IRandomValues<int> MultipleSetsSingleValue = RandomValues.Create(new[]
		{
			new[] { 1 },
			new[] { 2 }
		});

	private static readonly IRandomValues<int> MultipleSetsMultipleValues = RandomValues.Create(new[]
		{
			new[] { 1, 2 },
			new[] { 3, 4 }
		});
	// @formatter:on
#endregion Sample Data

	[SetUp]
	public void PreRun()
	{
		_sut = new ConsoleFormatter(A.Dummy<ConsoleFormatterOptions>());
	}

	[Test]
	public void Format_shall_return_empty_if_sets_are_null() =>
		_sut.Format(null!).IsEmpty.ShouldBeTrue();
	
	[Test]
	public void Format_shall_return_empty_result_if_sets_are_empty() =>
		_sut.Format(RandomValues.Create(new[]
				{
					new[]
						{
							""
						},
					null,
					new[]
						{
							(string?)null
						}
				}!))
			.ShouldNotBeNull()
			.ShouldBe(FormatContext.Empty);
	
	[Test]
	public void Format_shall_exclude_null_sets() =>
		_sut.Format(RandomValues.Create(new[]
				{
					new[]
						{
							"a"
						},
					null,
					new[]
						{
							"c"
						}
				}!)).Dump()?.Raw
			.ShouldBe("a\r\nc");

	[Test]
	public void Format_shall_exclude_empty_sets() =>
		_sut.Format(RandomValues.Create(new[]
				{
					new[]
						{
							"a"
						},
					new[]
						{
							"\t"
						},
					new[]
						{
							"c"
						}
				})).Dump()?.Raw
			.ShouldBe("a\r\nc");

	[Test]
	public void Format_shall_exclude_null_values() =>
		_sut.Format(RandomValues.Create(new[]
				{
					new[]
						{
							"a",
							null,
							"c"
						},
					new[]
						{
							"d",
							null,
							"f"
						}
				})).Dump()?.Raw
			.ShouldBe("[a, c]\r\n[d, f]");

	[Test]
	public void Format_shall_exclude_empty_values() =>
		_sut.Format(RandomValues.Create(new[]
				{
					new[]
						{
							"a",
							"\r\n",
							"c"
						},
					new[]
						{
							"d",
							"\t",
							"f"
						}
				})).Dump()?.Raw
			.ShouldBe("[a, c]\r\n[d, f]");

	[Test]
	public void Format_single_set_single_value_should_not_append_nor_suffix() =>
		_sut.Format(SingleSetSingleValue).Dump()?.Raw
			.ShouldBe("1");

	[Test]
	public void Format_single_set_multiple_values_should_not_append_nor_suffix() =>
		_sut.Format(SingleSetMultipleValues).Dump()?.Raw
			.ShouldBe("1\r\n2");

	[Test]
	public void Format_multiple_sets_single_value_should_not_append_nor_suffix() =>
		_sut.Format(MultipleSetsSingleValue).Dump()?.Raw
			.ShouldBe("1\r\n2");

	[Test]
	public void Format_multiple_sets_with_multiple_values_should_enclose_set() =>
		_sut.Format(MultipleSetsMultipleValues).Dump()?.Raw
			.ShouldBe("[1, 2]\r\n[3, 4]");

	[Test]
	public void FormatElement_shall_not_add_color_if_coloring_is_disabled() =>
		ConsoleFormatter.FormatElement(1, true).ShouldBe("1");

	[Test]
	public void FormatElement_shall_add_color_if_coloring_is_enabled() =>
		ConsoleFormatter.FormatElement(1, false).ShouldBe("\x1b[1;32m1\x1b[0m");

	[TestCase(null)]
	[TestCase("")]
	[TestCase("\t")]
	public void IsValidElement_shall_return_false_if_string_is_null_or_whitespace(string s) =>
		ConsoleFormatter.IsValidElement(s).ShouldBeFalse();

	[Test]
	public void IsValidElement_shall_return_false_if_element_is_null() =>
		ConsoleFormatter.IsValidElement<string>(null).ShouldBeFalse();
}