using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace RGen.Logic.Output;

public class ConsoleOutput : IOutput
{
	public Task WriteAsync<T>(IEnumerable<IEnumerable<T>> sets, CancellationToken cancellationToken)
	{
		foreach (var sequence in sets)
		{
			var array = sequence.ToArray();

			if (array.Length > 1)
				Console.WriteLine("[{0}]", string.Join(", ", array));
			else
				Console.WriteLine(array.Single());
		}

		return Task.CompletedTask;
	}
}