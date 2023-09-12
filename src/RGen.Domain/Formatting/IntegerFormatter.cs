using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using RGen.Domain.Generating;
using RGen.Domain.Generating.Generators;


namespace RGen.Domain.Formatting;


public class IntegerFormatter : IFormatter
{
	private readonly ILogger<IntegerFormatter> _logger;
	private readonly IntegerFormatterOptions _options;

	public IntegerFormatter(ILogger<IntegerFormatter> logger, IntegerFormatterOptions options)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

//TODO: This shouldn't have to create a new IRV and box types etc...
	public IRandomValues<string> Format(IRandomValues randomValues) =>
		new RandomValues<string>(randomValues.ValueSets.Select(s => s.Select(e => FormatElement(_options.Base, (ulong)e))));

//TODO: Optimize with string.Create() and spans
	internal string FormatElement(IntegerBase format, ulong element)
	{
		try
		{
			return format switch
				{
					IntegerBase.Decimal     => element.ToString("D"),
					IntegerBase.Hexadecimal => element.ToString("x"),
					IntegerBase.Binary      => FormatAsBitString(element),
					_                       => element.ToString()
				};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to format {Value} as {Format}", element, format);
			return element.ToString();
		}
	}

	internal static string FormatAsBitString(ulong n) =>
		Convert.ToString(unchecked((long)n), 2) // Cast to long, as the number of bits are the same as with ulong
			.PadLeft((int)Math.Max(1, Math.Ceiling(MathUtils.CountNumberOfBits(n) / 8m)) * 8, '0'); // Add leading zeros to chunk it into pretty bytes
}