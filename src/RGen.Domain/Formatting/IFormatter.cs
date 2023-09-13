using RGen.Domain.Generating;


namespace RGen.Domain.Formatting;


public interface IFormatter
{
	IRandomValues<string> Format(IRandomValues randomValues);
}