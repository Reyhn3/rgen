﻿using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RGen.Application.Formatting;
using RGen.Application.Formatting.Console;
using RGen.Application.Writing;
using RGen.Application.Writing.Console;
using RGen.Application.Writing.TextFile;
using RGen.Domain.Generators;


namespace RGen.Application.Commanding.Integer;

public class GenerateIntegerHandler : GlobalCommandHandler
{
	private readonly IIntegerGenerator _generator;
	private readonly IFormatterFactory _formatterFactory;
	private readonly IWriterFactory _writerFactory;

	public GenerateIntegerHandler(
		IIntegerGenerator generator, 
		IFormatterFactory formatterFactory, 
		IWriterFactory writerFactory)
	{
		_generator = generator ?? throw new ArgumentNullException(nameof(generator));
		_formatterFactory = formatterFactory ?? throw new ArgumentNullException(nameof(formatterFactory));
		_writerFactory = writerFactory ?? throw new ArgumentNullException(nameof(writerFactory));
	}

	public int N { get; set; }
	public int Set { get; set; }

	protected override async Task<ExitCode> InvokeCoreAsync(InvocationContext context)
	{
//TODO: Refactor to be a chained process, e.g. generate -> format -> output

		var sets = _generator.Set(N, Set);

		var formatter = _formatterFactory.Create(new ConsoleFormatterOptions(NoColor));
		var formatted = formatter.Format(sets);
		if (formatted.IsEmpty)
			return ExitCode.NoDataGenerated;

//TODO: Get CT from call-chain
		var consoleResult = await WriteToConsole(formatted.Formatted, CancellationToken.None);
		if (consoleResult != ExitCode.OK)
			return consoleResult;

//TODO: Get CT from call-chain
		var outputResult = await WriteToOutput(formatted.Raw, Output, CancellationToken.None);
		if (outputResult != ExitCode.OK)
			return outputResult;

		return ExitCode.OK;
	}

	private async Task<ExitCode> WriteToConsole(string content, CancellationToken cancellationToken)
	{
		var writer = _writerFactory.Create(new ConsoleWriterOptions());
		var writeResult = await writer.WriteAsync(content, cancellationToken);
		return writeResult;
	}

	private async Task<ExitCode> WriteToOutput(string content, FileInfo? output, CancellationToken cancellationToken)
	{
		if (output == null)
			return ExitCode.OK;

		var writer = _writerFactory.Create(new PlainTextFileWriterOptions(output));
		var writeResult = await writer.WriteAsync(content, cancellationToken);
		return writeResult;
	}
}