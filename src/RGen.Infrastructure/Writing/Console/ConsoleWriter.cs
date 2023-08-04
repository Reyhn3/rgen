﻿using System.Threading;
using System.Threading.Tasks;
using RGen.Domain;
using RGen.Domain.Writing;
using StdOut = System.Console;


namespace RGen.Infrastructure.Writing.Console;

public class ConsoleWriter : IWriter
{
	private readonly ConsoleWriterOptions _options;

	public ConsoleWriter(ConsoleWriterOptions options)
	{
		_options = options;
	}

	public Task<IResult> WriteAsync(string values, CancellationToken cancellationToken)
	{
		StdOut.WriteLine(values);
		return Task.FromResult(Result.OK);
	}
}