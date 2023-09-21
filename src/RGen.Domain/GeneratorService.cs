using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RGen.Domain.Formatting;
using RGen.Domain.Generating;
using RGen.Domain.Rendering;
using RGen.Domain.Writing;


namespace RGen.Domain;


public class GeneratorService : IGeneratorService
{
	public async Task<IResult> GenerateAsync(
		IGenerator generator,
		IFormatter formatter,
		IRenderer renderer,
		IEnumerable<IWriter> writers,
		int numberOfElements,
		int numberOfSets,
		object? parameters,
		CancellationToken cancellationToken = default)
	{
		var sets = generator.Generate(numberOfElements, numberOfSets, parameters);
		var formatted = formatter.Format(sets);
		var rendered = renderer.Render(numberOfSets, formatted);
		if (rendered.IsEmpty)
			return Result.Failure(ResultCode.NoDataGenerated);

		var failedWriteResults = new List<IResult>();
		foreach (var writer in writers)
		{
			var writeResult = await writer.WriteAsync(rendered, cancellationToken);
			if (!writeResult.IsSuccessful)
				failedWriteResults.Add(writeResult);
		}

//TODO: Replace this with an aggregated result
		if (failedWriteResults.Any())
			return Result.Failure(ResultCode.WriteError, "One or more writers failed");

		return Result.OK;
	}
}