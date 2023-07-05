using System;
using System.Collections.Generic;
using System.Text;


namespace RGen.Logic.Formatting;

public class ConsoleFormatter : IFormatter
{
	private const char BeginArray = '[';
	private const char EndArray = ']';

	private static readonly string SetSeparator = Environment.NewLine;
	private static readonly string ElementSeparator = ", ";

	public string Format<T>(IEnumerable<IEnumerable<T>> sets)
	{
		var sb = new StringBuilder();

		var setCount = 0;
		var elementCount = 0;

		foreach (var set in sets)
		{
			setCount++;

			if (setCount > 1)
			{
				sb.Append(SetSeparator);
				sb.Append(BeginArray);
			}

			foreach (var element in set)
			{
				elementCount++;

				var formatted = Format(element);
				if (elementCount > 1)
					sb.Append($"{ElementSeparator}{formatted}");
				else
					sb.Append(formatted);
			}

			sb.Append(EndArray);
			elementCount = 0;
		}

		if (setCount > 1)
			sb.Insert(0, BeginArray);

		return sb.ToString();
	}

//TODO: Color, if enabled
	private string Format<T>(T element) =>
		element.ToString();
}