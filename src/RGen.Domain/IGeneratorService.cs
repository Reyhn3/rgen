using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RGen.Domain.Formatting;
using RGen.Domain.Generating;
using RGen.Domain.Writing;


namespace RGen.Domain;

public interface IGeneratorService
{
	Task<IResult> GenerateAsync(
		IGenerator generator,
		IFormatter formatter,
		IEnumerable<IWriter> writers,
		int numberOfElements,
		int numberOfSets,
		CancellationToken cancellationToken = default);
}