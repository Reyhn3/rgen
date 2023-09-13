using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RGen.Domain.Formatting;
using RGen.Domain.Generating;
using RGen.Domain.Rendering;
using RGen.Domain.Writing;


namespace RGen.Domain;


public interface IGeneratorService
{
//TODO: #42: Refactor to use parameter-object instead of list of parameters
	Task<IResult> GenerateAsync(
		IGenerator generator,
		IFormatter formatter,
		IRenderer renderer,
		IEnumerable<IWriter> writers,
		int numberOfElements,
		int numberOfSets,
		int? length,
		ulong? min,
		ulong? max,
		CancellationToken cancellationToken = default);
}