using System;
using System.Linq;
using RGen.Domain.Generating;
using RGen.Domain.Generating.Generators;


namespace RGen.Domain.Formatting;


public class IntegerFormatter : IFormatter
{
	private readonly IntegerFormatterOptions _options;

	public IntegerFormatter(IntegerFormatterOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

//TODO: This shouldn't have to create a new IRV and box types etc...
	public IRandomValues<string> Format(IRandomValues randomValues) =>
		new RandomValues<string>(randomValues.ValueSets.Select(s => s.Select(e => FormatElement(_options.Format, (long)e))));

	internal static string FormatElement(IntegerFormat format, long element) =>
		format switch
			{
				IntegerFormat.Decimal     => element.ToString("D"),
				IntegerFormat.Hexadecimal => element.ToString("x"),
				IntegerFormat.Binary      => Convert.ToString(element, 2),
				_                         => element.ToString()
			};
}