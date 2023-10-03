using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using RGen.Domain.Formatting;
using RGen.Infrastructure.Rendering.Console;
using Shouldly;


namespace RGen.Infrastructure.Tests.Rendering.Console;


public class ConsoleRendererTests
{
	private ConsoleRenderer _sut = null!;

#region Sample Data
	private static readonly IEnumerable<FormattedRandomValue> SingleValue = new FormattedRandomValue[]
		{
			new(A.Dummy<object>(), "1")
		};

	private static readonly IEnumerable<FormattedRandomValue> DoubleValues = new FormattedRandomValue[]
		{
			new(A.Dummy<object>(), "1"),
			new(A.Dummy<object>(), "2")
		};

	private static readonly IEnumerable<FormattedRandomValue> UnevenValues = new FormattedRandomValue[]
		{
			new(A.Dummy<object>(), "1"),
			new(A.Dummy<object>(), "2"),
			new(A.Dummy<object>(), "3")
		};

	private static readonly IEnumerable<FormattedRandomValue> MultipleValues = new FormattedRandomValue[]
		{
			new(A.Dummy<object>(), "1"),
			new(A.Dummy<object>(), "2"),
			new(A.Dummy<object>(), "3"),
			new(A.Dummy<object>(), "4")
		};

	private static readonly IEnumerable<FormattedRandomValue> ManyValues = new FormattedRandomValue[]
		{
			new(A.Dummy<object>(), "1"),
			new(A.Dummy<object>(), "2"),
			new(A.Dummy<object>(), "3"),
			new(A.Dummy<object>(), "4"),
			new(A.Dummy<object>(), "5"),
			new(A.Dummy<object>(), "6"),
			new(A.Dummy<object>(), "7"),
			new(A.Dummy<object>(), "8"),
			new(A.Dummy<object>(), "9"),
			new(A.Dummy<object>(), "10"),
			new(A.Dummy<object>(), "11"),
			new(A.Dummy<object>(), "12"),
			new(A.Dummy<object>(), "13"),
			new(A.Dummy<object>(), "14"),
			new(A.Dummy<object>(), "15"),
			new(A.Dummy<object>(), "16")
		};
#endregion Sample Data

	[SetUp]
	public void PreRun()
	{
		_sut = new ConsoleRenderer();
	}

	[Test]
	public void Render_shall_return_empty_if_sets_are_null() =>
		_sut.Render(1, null!, A.Dummy<ConsoleRendererOptions>()).IsEmpty.ShouldBeTrue();

	[Test]
	public void Render_shall_exclude_null_values() =>
		_sut.Render(1,
				new[]
					{
						new FormattedRandomValue(A.Dummy<object>(), "a"),
						null,
						new FormattedRandomValue(A.Dummy<object>(), "c")
					}!,
				A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("a\r\nc");

	[Test]
	public void Render_shall_exclude_empty_values() =>
		_sut.Render(1,
				new[]
					{
						new FormattedRandomValue(A.Dummy<object>(), "a"),
						new FormattedRandomValue(A.Dummy<object>(), string.Empty),
						new FormattedRandomValue(A.Dummy<object>(), "c")
					}!,
				A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("a\r\nc");

	[Test]
	public void Render_shall_exclude_null_string_values() =>
		_sut.Render(1,
				new[]
					{
						new FormattedRandomValue(A.Dummy<object>(), "a"),
						new FormattedRandomValue(A.Dummy<object>(), null!),
						new FormattedRandomValue(A.Dummy<object>(), "c")
					}!,
				A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("a\r\nc");

	[Test]
	public void Render_single_set_single_value_should_not_append_nor_suffix() =>
		_sut.Render(1, SingleValue, A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("1");

	[Test]
	public void Render_single_set_multiple_values_should_not_append_nor_suffix() =>
		_sut.Render(2, DoubleValues, A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("1\r\n2");

	[Test]
	public void Render_multiple_sets_single_value_should_not_append_nor_suffix() =>
		_sut.Render(1, DoubleValues, A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("1\r\n2");

	[Test]
	public void Render_multiple_sets_with_multiple_values_should_enclose_set() =>
		_sut.Render(2, MultipleValues, A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("[1, 2]\r\n[3, 4]");

	[Test]
	public void Render_many_elements_should_be_correctly_rendered() =>
		_sut.Render(4, ManyValues, A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("[1, 2, 3, 4]\r\n[5, 6, 7, 8]\r\n[9, 10, 11, 12]\r\n[13, 14, 15, 16]");

	[Test]
	public void Render_should_handle_multiple_sets_with_uneven_multiset_calculation() =>
		_sut.Render(2, UnevenValues, A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("[1, 2]\r\n[3]");

	[Test]
	public void Render_should_handle_jagged_sets() =>
		_sut.Render(3, MultipleValues, A.Dummy<ConsoleRendererOptions>()).Dump()?.Raw
			.ShouldBe("[1, 2, 3]\r\n[4]");

	[Test]
	public void RenderElement_shall_not_add_color_if_coloring_is_disabled() =>
		ConsoleRenderer.RenderElement(1, true).ShouldBe("1");

	[Test]
	public void RenderElement_shall_add_color_if_coloring_is_enabled() =>
		ConsoleRenderer.RenderElement(1, false).ShouldBe("\x1b[1;32m1\x1b[0m");

	[TestCase(null)]
	[TestCase("")]
	[TestCase("\t")]
	public void IsValidElement_shall_return_false_if_string_is_null_or_whitespace(string s) =>
		ConsoleRenderer.IsValidElement(new FormattedRandomValue(A.Dummy<object>(), s)).ShouldBeFalse();

	[Test]
	public void IsValidElement_shall_return_false_if_element_is_null() =>
		ConsoleRenderer.IsValidElement(null!).ShouldBeFalse();
}