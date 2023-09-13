using RGen.Domain.Formatting;


namespace RGen.Application.Formatting;


public interface IFormatterFactory
{
	IFormatter Create<TOptions>(TOptions options)
		where TOptions : IFormatterOptions;
}