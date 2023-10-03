using System.Collections.Generic;
using RGen.Domain.Formatting;


namespace RGen.Domain.Rendering;


public interface IRenderer
{
	RenderContext Render(
		int numberOfElementsPerSet,
		IEnumerable<FormattedRandomValue> randomValues,
		object? parameters);
}


public interface IRenderer<in TParams> : IRenderer
	where TParams : struct
{
	RenderContext Render(
		int numberOfElementsPerSet,
		IEnumerable<FormattedRandomValue> randomValues,
		TParams parameters);
}