using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;


namespace RGen.Logic.Integer;

public class GenerateIntegerHandler : GlobalCommandHandler
{
	public int N { get; set; }
	public int Set { get; set; }

	protected override async Task<int> InvokeCoreAsync(InvocationContext context)
	{
//TODO: Validate boundaries (Set should be >= 1)
		if (Set == 1)
		{
			var numbers = await Generate(N);
			foreach (var number in numbers)
				Console.WriteLine(number);
		}
		else
		{
			var sets = await Task.WhenAll(Enumerable.Range(0, Set).Select(_ => Generate(N)));
			foreach (var set in sets)
//TODO: Extract formatting to separate class
				Console.WriteLine("[{0}]", string.Join(", ", set));
		}

		return 0;
	}

	private static async Task<IEnumerable<int>> Generate(int n) =>
		await Task.WhenAll(Enumerable.Range(0, n).Select(_ => Generate()));

//TODO: Generate actual random integer
	private static async Task<int> Generate()
	{
		await Task.Delay(10);
		return DateTime.Now.Microsecond;
	}
}