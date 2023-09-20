using System.Collections.Generic;
using RGen.Domain.Formatting;


namespace RGen.Domain.Rendering;


public interface IRenderer
{
	RenderContext Render(int numberOfElementsPerSet, IEnumerable<FormattedRandomValue> randomValues);
}