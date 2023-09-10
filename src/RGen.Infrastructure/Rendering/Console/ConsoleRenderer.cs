using System;
using System.Linq;
using System.Text;
using RGen.Domain.Generating;
using RGen.Domain.Rendering;
using RGen.Infrastructure.Logging;


namespace RGen.Infrastructure.Rendering.Console;


public class ConsoleRenderer : IRenderer
{
	private const string NullElement = "<null>";
	private const char BeginArray = '[';
	private const char EndArray = ']';

	private static readonly string SetSeparator = Environment.NewLine;
	private static readonly string ElementSeparator = ", ";

	private readonly bool _isColoringDisabled;

	public ConsoleRenderer(ConsoleRendererOptions options)
	{
		_isColoringDisabled = options.IsColoringDisabled || LogHelper.IsNoColorSet;
	}

	public RenderContext Render(IRandomValues randomValues)
	{
		if (randomValues?.ValueSets == null!)
			return RenderContext.Empty;

		var array = randomValues.ValueSets
			.Where(s => s != null!)
			.Select(s => s.Where(IsValidElement).ToArray())
			.Where(s => s.Any())
			.ToArray();
		if (!array.Any())
			return RenderContext.Empty;

		var isMultiSet = array.Length > 1;
		var isMultiElement = array.First().Length > 1;

		var rawStringBuilder = new StringBuilder();
		var renderedStringBuilder = new StringBuilder();

		for (var i = 0; i < array.Length; i++)
		{
			var set = array[i];
			if (!set.Any())
				continue;

			if (isMultiSet && isMultiElement)
			{
				rawStringBuilder.Append(BeginArray);
				renderedStringBuilder.Append(BeginArray);
			}

			for (var j = 0; j < set.Length; j++)
			{
				var element = set[j];
				var rendered = RenderElement(element, _isColoringDisabled);
				rawStringBuilder.Append(element);
				renderedStringBuilder.Append(rendered);

				if (j < set.Length - 1)
				{
					rawStringBuilder.Append(isMultiSet ? ElementSeparator : SetSeparator);
					renderedStringBuilder.Append(isMultiSet ? ElementSeparator : SetSeparator);
				}
			}

			if (isMultiSet && isMultiElement)
			{
				rawStringBuilder.Append(EndArray);
				renderedStringBuilder.Append(EndArray);
			}

			if (i < array.Length - 1)
			{
				rawStringBuilder.Append(SetSeparator);
				renderedStringBuilder.Append(SetSeparator);
			}
		}

		return new RenderContext(rawStringBuilder.ToString(), renderedStringBuilder.ToString());
	}

	internal static bool IsValidElement<T>(T? element)
	{
		if (element is string s)
			return !string.IsNullOrWhiteSpace(s);

		if (element is null)
			return false;

		return true;
	}

	internal static string RenderElement<T>(T element, bool isColoringDisabled) =>
		isColoringDisabled
			? element?.ToString() ?? NullElement
			: $"\x1b[1;32m{element}\x1b[0m";
}