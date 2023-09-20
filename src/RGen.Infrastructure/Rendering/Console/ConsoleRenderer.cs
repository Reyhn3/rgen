using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RGen.Domain.Formatting;
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

	public RenderContext Render(int numberOfElementsPerSet, IEnumerable<FormattedRandomValue> randomValues)
	{
		var array = (randomValues?.Where(IsValidElement) ?? Enumerable.Empty<FormattedRandomValue>()).ToArray();
		if (!array.Any())
			return RenderContext.Empty;

		var isMultiSet = Math.Ceiling(array.Length / (decimal)numberOfElementsPerSet) > 1;
		var isMultiElement = numberOfElementsPerSet > 1;

		var rawStringBuilder = new StringBuilder();
		var renderedStringBuilder = new StringBuilder();

		for (var i = 0; i < array.Length; i += numberOfElementsPerSet)
		{
			var set = array[i..(Math.Min(array.Length, i + numberOfElementsPerSet))];
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
				var formatted = element.Formatted;
				var rendered = RenderElement(formatted, _isColoringDisabled);
				rawStringBuilder.Append(formatted);
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

			if (i + set.Length < array.Length)
			{
				rawStringBuilder.Append(SetSeparator);
				renderedStringBuilder.Append(SetSeparator);
			}
		}

		return new RenderContext(rawStringBuilder.ToString(), renderedStringBuilder.ToString());
	}

	internal static bool IsValidElement(FormattedRandomValue element)
	{
		if (element is null)
			return false;

		return !string.IsNullOrWhiteSpace(element.Formatted);
	}

	internal static string RenderElement<T>(T element, bool isColoringDisabled) =>
		isColoringDisabled
			? element?.ToString() ?? NullElement
			: $"\x1b[1;32m{element}\x1b[0m";
}