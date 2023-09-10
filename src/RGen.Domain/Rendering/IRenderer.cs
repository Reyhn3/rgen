using RGen.Domain.Generating;


namespace RGen.Domain.Rendering;

public interface IRenderer
{
	RenderContext Render(IRandomValues randomValues);
}