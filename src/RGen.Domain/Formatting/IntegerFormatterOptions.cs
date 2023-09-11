using RGen.Domain.Generating.Generators;


namespace RGen.Domain.Formatting;


public class IntegerFormatterOptions : IFormatterOptions
{
	public IntegerFormatterOptions(IntegerBase @base)
	{
		Base = @base;
	}

	public IntegerBase Base { get; }
}