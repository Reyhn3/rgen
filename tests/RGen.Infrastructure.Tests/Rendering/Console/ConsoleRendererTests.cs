using FakeItEasy;
using NUnit.Framework;
using RGen.Domain.Generating;
using RGen.Domain.Rendering;
using RGen.Infrastructure.Rendering.Console;
using Shouldly;


namespace RGen.Infrastructure.Tests.Rendering.Console;

public class ConsoleRendererTests
{
	private ConsoleRenderer _sut = null!;

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
		_sut = new ConsoleRenderer(A.Dummy<ConsoleRendererOptions>());
	}

	[Test]
	public void Render_shall_return_empty_if_sets_are_null() =>
		_sut.Render(null!).IsEmpty.ShouldBeTrue();
	
	[Test]
	public void Render_shall_return_empty_result_if_sets_are_empty() =>
		_sut.Render(RandomValues.Create(new[]
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
			.ShouldBe(RenderContext.Empty);
	
	[Test]
	public void Render_shall_exclude_null_sets() =>
		_sut.Render(RandomValues.Create(new[]
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
	public void Render_shall_exclude_empty_sets() =>
		_sut.Render(RandomValues.Create(new[]
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
	public void Render_shall_exclude_null_values() =>
		_sut.Render(RandomValues.Create(new[]
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
	public void Render_shall_exclude_empty_values() =>
		_sut.Render(RandomValues.Create(new[]
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
	public void Render_single_set_single_value_should_not_append_nor_suffix() =>
		_sut.Render(SingleSetSingleValue).Dump()?.Raw
			.ShouldBe("1");

	[Test]
	public void Render_single_set_multiple_values_should_not_append_nor_suffix() =>
		_sut.Render(SingleSetMultipleValues).Dump()?.Raw
			.ShouldBe("1\r\n2");

	[Test]
	public void Render_multiple_sets_single_value_should_not_append_nor_suffix() =>
		_sut.Render(MultipleSetsSingleValue).Dump()?.Raw
			.ShouldBe("1\r\n2");

	[Test]
	public void Render_multiple_sets_with_multiple_values_should_enclose_set() =>
		_sut.Render(MultipleSetsMultipleValues).Dump()?.Raw
			.ShouldBe("[1, 2]\r\n[3, 4]");

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
		ConsoleRenderer.IsValidElement(s).ShouldBeFalse();

	[Test]
	public void IsValidElement_shall_return_false_if_element_is_null() =>
		ConsoleRenderer.IsValidElement<string>(null).ShouldBeFalse();
}