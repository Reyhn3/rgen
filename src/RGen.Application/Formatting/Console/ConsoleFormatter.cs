using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RGen.Application.Formatting.Console;

public class ConsoleFormatter : IFormatter
{
	private const char BeginArray = '[';
	private const char EndArray = ']';

	private static readonly string SetSeparator = Environment.NewLine;
	private static readonly string ElementSeparator = ", ";

	private readonly bool _isColoringDisabled;

	public ConsoleFormatter(ConsoleFormatterOptions options)
	{
		_isColoringDisabled = options.IsColoringDisabled;
	}

	public string Format<T>(IEnumerable<IEnumerable<T>> sets)
	{
		var sb = new StringBuilder();

		var array = sets.Select(s => s.ToArray()).ToArray();
		var isMultiSet = array.Length > 1;
		var isMultiElement = array.First().Length > 1;

		for (var i = 0; i < array.Length; i++)
		{
			var set = array[i];
			if (isMultiSet && isMultiElement)
				sb.Append(BeginArray);

			for (var j = 0; j < set.Length; j++)
			{
				var element = set[j];
				var formatted = Format(element, _isColoringDisabled);
				sb.Append(formatted);

				if (j < set.Length - 1)
					sb.Append(isMultiSet ? ElementSeparator : SetSeparator);
			}

			if (isMultiSet && isMultiElement)
				sb.Append(EndArray);

			if (i < array.Length - 1)
				sb.Append(SetSeparator);
		}

		return sb.ToString();
	}

//TEST: Coloring
	private static string Format<T>(T element, bool isColoringDisabled) =>
		isColoringDisabled
			? element.ToString()
			: $"\x1b[1;32m{element}\x1b[0m";
}