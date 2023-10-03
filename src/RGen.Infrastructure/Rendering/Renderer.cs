using System.Collections.Generic;
using RGen.Domain.Formatting;
using RGen.Domain.Rendering;


namespace RGen.Infrastructure.Rendering;


public abstract class Renderer<TParams> : IRenderer<TParams>
	where TParams : struct
{
	RenderContext IRenderer.Render(int numberOfElementsPerSet, IEnumerable<FormattedRandomValue> randomValues, object? parameters) =>
		Render(numberOfElementsPerSet, randomValues, (TParams)(parameters ?? default(TParams)));

	public abstract RenderContext Render(int numberOfElementsPerSet, IEnumerable<FormattedRandomValue> randomValues, TParams parameters);
}