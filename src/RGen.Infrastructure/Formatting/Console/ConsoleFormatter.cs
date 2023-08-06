using System;
using System.Linq;
using System.Text;
using RGen.Domain.Formatting;
using RGen.Domain.Generating;


namespace RGen.Infrastructure.Formatting.Console;

public class ConsoleFormatter : IFormatter
{
	private const string NullElement = "<null>";
	private const char BeginArray = '[';
	private const char EndArray = ']';

	private static readonly string SetSeparator = Environment.NewLine;
	private static readonly string ElementSeparator = ", ";

	private readonly bool _isColoringDisabled;

	public ConsoleFormatter(ConsoleFormatterOptions options)
	{
		_isColoringDisabled = options.IsColoringDisabled;
	}

	public FormatContext Format(IRandomValues randomValues)
	{
		if (randomValues?.ValueSets == null!)
			return FormatContext.Empty;

		var array = randomValues.ValueSets
			.Where(s => s != null!)
			.Select(s => s.Where(IsValidElement).ToArray())
			.Where(s => s.Any())
			.ToArray();
		if (!array.Any())
			return FormatContext.Empty;

		var isMultiSet = array.Length > 1;
		var isMultiElement = array.First().Length > 1;

		var rawStringBuilder = new StringBuilder();
		var formattedStringBuilder = new StringBuilder();

		for (var i = 0; i < array.Length; i++)
		{
			var set = array[i];
			if (!set.Any())
				continue;

			if (isMultiSet && isMultiElement)
			{
				rawStringBuilder.Append(BeginArray);
				formattedStringBuilder.Append(BeginArray);
			}

			for (var j = 0; j < set.Length; j++)
			{
				var element = set[j];
				var formatted = FormatElement(element, _isColoringDisabled);
				rawStringBuilder.Append(element);
				formattedStringBuilder.Append(formatted);

				if (j < set.Length - 1)
				{
					rawStringBuilder.Append(isMultiSet ? ElementSeparator : SetSeparator);
					formattedStringBuilder.Append(isMultiSet ? ElementSeparator : SetSeparator);
				}
			}

			if (isMultiSet && isMultiElement)
			{
				rawStringBuilder.Append(EndArray);
				formattedStringBuilder.Append(EndArray);
			}

			if (i < array.Length - 1)
			{
				rawStringBuilder.Append(SetSeparator);
				formattedStringBuilder.Append(SetSeparator);
			}
		}

		return new FormatContext(rawStringBuilder.ToString(), formattedStringBuilder.ToString());
	}

	internal static bool IsValidElement<T>(T? element)
	{
		if (element is string s)
			return !string.IsNullOrWhiteSpace(s);

		if (element is null)
			return false;

		return true;
	}

	internal static string FormatElement<T>(T element, bool isColoringDisabled) =>
		isColoringDisabled
			? element?.ToString() ?? NullElement
			: $"\x1b[1;32m{element}\x1b[0m";
}