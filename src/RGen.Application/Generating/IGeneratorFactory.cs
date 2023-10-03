using RGen.Domain.Generating;


namespace RGen.Application.Generating;


public interface IGeneratorFactory
{
	IGenerator Create<TOptions>(TOptions options)
		where TOptions : IGeneratorOptions;
}