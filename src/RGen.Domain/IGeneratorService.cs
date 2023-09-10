using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RGen.Domain.Generating;
using RGen.Domain.Generating.Generators;
using RGen.Domain.Rendering;
using RGen.Domain.Writing;


namespace RGen.Domain;

public interface IGeneratorService
{
//TODO: #42: Refactor to use parameter-object instead of list of parameters
	Task<IResult> GenerateAsync(
		IGenerator generator,
		IRenderer renderer,
		IEnumerable<IWriter> writers,
		int numberOfElements,
		int numberOfSets,
		int? length,
		int? min,
		int? max,
//TODO: #42: Obviously, this shouldn't be here on a value-agnostic interface
		IntegerFormat format,
		CancellationToken cancellationToken = default);
}