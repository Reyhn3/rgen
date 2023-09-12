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
		new RandomValues<string>(randomValues.ValueSets.Select(s => s.Select(e => FormatElement(_options.Base, (long)e))));

//TODO: Optimize with string.Create() and spans
	internal static string FormatElement(IntegerBase format, long element)
	{
		try
		{
			return format switch
				{
					IntegerBase.Decimal     => element.ToString("D"),
					IntegerBase.Hexadecimal => element.ToString("x"),
					IntegerBase.Binary      => Convert.ToString(element, 2).PadLeft((int)(Math.Max(1, Math.Ceiling(Math.Log2(element) / 8)) * 8), '0'),
					_                       => element.ToString()
				};
		}
		catch (Exception e)
		{
			Console.WriteLine(element);
			Console.WriteLine((int)(Math.Max(1, Math.Ceiling(Math.Log2(Math.Abs(element)) / 8)) * 8));
			Console.WriteLine(e);
			throw;
		}
	}
	
}