using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RGen.Domain.Generating.Generators;


namespace RGen.Domain.Formatting;


public class IntegerFormatter : Formatter<ulong, IntegerFormatterOptions>
{
	private readonly ILogger<IntegerFormatter> _logger;

	public IntegerFormatter(ILogger<IntegerFormatter> logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public override IEnumerable<FormattedRandomValue> Format(IEnumerable<ulong> randomValues, IntegerFormatterOptions parameters) =>
		randomValues.Select(v => new FormattedRandomValue(v, FormatElement(parameters.Base, v)));


//TODO: Optimize with string.Create() and spans
	internal string FormatElement(IntegerBase @base, ulong element)
	{
		try
		{
			return @base switch
				{
					IntegerBase.Decimal     => element.ToString("D"),
					IntegerBase.Hexadecimal => element.ToString("x"),
					IntegerBase.Binary      => FormatAsBitString(element),
					_                       => element.ToString()
				};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to format {Value} as {Base}", element, @base);
			return element.ToString();
		}
	}

	internal static string FormatAsBitString(ulong n) =>
		Convert.ToString(unchecked((long)n), 2)                                                     // Cast to long, as the number of bits are the same as with ulong
			.PadLeft((int)Math.Max(1, Math.Ceiling(MathUtils.CountNumberOfBits(n) / 8m)) * 8, '0'); // Add leading zeros to chunk it into pretty bytes
}