using RGen.Domain.Rendering;


namespace RGen.Application.Rendering;

public interface IRendererFactory
{
	IRenderer Create<TOptions>(TOptions options)
		where TOptions : IRendererOptions;
}