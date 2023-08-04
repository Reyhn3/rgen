using RGen.Domain.Generating;


namespace RGen.Domain.Formatting;

public interface IFormatter
{
	FormatContext Format(IRandomValues randomValues);
}