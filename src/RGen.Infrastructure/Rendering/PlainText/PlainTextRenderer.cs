using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RGen.Domain.Formatting;
using RGen.Domain.Rendering;


namespace RGen.Infrastructure.Rendering.PlainText;


public class PlainTextRenderer : Renderer<PlainTextRendererOptions>
{
	private const string NullElement = "<null>";
	private const char BeginArray = '[';
	private const char EndArray = ']';

	private static readonly string SetSeparator = Environment.NewLine;
	private static readonly string ElementSeparator = ", ";

	public override RenderContext Render(int numberOfElementsPerSet, IEnumerable<FormattedRandomValue> randomValues, PlainTextRendererOptions options)
	{
		var array = (randomValues?.Where(IsValidElement) ?? Enumerable.Empty<FormattedRandomValue>()).ToArray();
		if (!array.Any())
			return RenderContext.Empty;

		var isMultiSet = Math.Ceiling(array.Length / (decimal)numberOfElementsPerSet) > 1;
		var isMultiElement = numberOfElementsPerSet > 1;

		var renderedStringBuilder = new StringBuilder();


		for (var i = 0; i < array.Length; i += numberOfElementsPerSet)
		{
			var set = array[i..Math.Min(array.Length, i + numberOfElementsPerSet)];
			if (!set.Any())
				continue;

			if (isMultiSet && isMultiElement)
				renderedStringBuilder.Append(BeginArray);

			for (var j = 0; j < set.Length; j++)
			{
				var element = set[j];
				var formatted = element.Formatted;
				var rendered = RenderElement(formatted);
				renderedStringBuilder.Append(rendered);

				if (j < set.Length - 1)
					renderedStringBuilder.Append(isMultiSet ? ElementSeparator : SetSeparator);
			}

			if (isMultiSet && isMultiElement)
				renderedStringBuilder.Append(EndArray);

			if (i + set.Length < array.Length)
				renderedStringBuilder.Append(SetSeparator);
		}

		var final = renderedStringBuilder.ToString();
		return new RenderContext(final, final);
	}

	internal static bool IsValidElement(FormattedRandomValue element)
	{
		if (element is null)
			return false;

		return !string.IsNullOrWhiteSpace(element.Formatted);
	}

	internal static string RenderElement<T>(T element) =>
		element?.ToString() ?? NullElement;
}