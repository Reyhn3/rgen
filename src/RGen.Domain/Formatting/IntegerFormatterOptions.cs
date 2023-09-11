using RGen.Domain.Generating.Generators;


namespace RGen.Domain.Formatting;


public class IntegerFormatterOptions : IFormatterOptions
{
	public IntegerFormatterOptions(IntegerFormat format)
	{
		Format = format;
	}

	public IntegerFormat Format { get; }
}