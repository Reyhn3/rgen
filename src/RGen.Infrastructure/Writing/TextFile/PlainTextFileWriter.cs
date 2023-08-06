﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RGen.Domain;
using RGen.Domain.Formatting;
using RGen.Domain.Writing;
using StdOut = System.Console;


namespace RGen.Infrastructure.Writing.TextFile;

public class PlainTextFileWriter : IWriter
{
	private readonly ILogger<PlainTextFileWriter> _logger;
	private readonly PlainTextFileWriterOptions _options;

	public PlainTextFileWriter(ILogger<PlainTextFileWriter> logger, PlainTextFileWriterOptions options)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_options = options;
	}

	public async Task<IResult> WriteAsync(FormatContext context, CancellationToken cancellationToken)
	{
		if (!TryGetOrCreateFileName(_options.FileName, out var filename))
			return Result.Failure(ResultCode.OutputFilePathError);

		if (!await TryWriteContentToFileAsync(filename!, context.Raw, Encoding.UTF8, cancellationToken))
			return Result.Failure(ResultCode.OutputFileWriteError);

		StdOut.WriteLine();
		StdOut.WriteLine("Values exported to:");
		StdOut.ForegroundColor = ConsoleColor.White;
		StdOut.WriteLine(filename);
		StdOut.ResetColor();

		return Result.OK;
	}

	internal bool TryGetOrCreateFileName(FileInfo? optionsFileName, out string? filename)
	{
		try
		{
			if (optionsFileName != null)
			{
				var pathAndFilename = Path.GetFullPath(optionsFileName.ToString());
				if (IsInvalidPath(pathAndFilename))
				{
					filename = null;
					return false;
				}

				var pathOnly = Path.GetDirectoryName(pathAndFilename);
				if (pathOnly != null)
					Directory.CreateDirectory(pathOnly);

				filename = pathAndFilename;
				return true;
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Specified file name and path is invalid");
			ConsoleHelper.PrintExceptionDetails(ex);
			filename = null;
			return false;
		}

		try
		{
			var tempFileName = Path.GetTempFileName();
			filename = Path.ChangeExtension(tempFileName, "txt");
			Debug.WriteLine($"Created temporary file '{filename}'");

			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error constructing file path and name for output");
			ConsoleHelper.PrintExceptionDetails(ex);
			filename = null;
			return false;
		}
	}

	private static bool IsInvalidPath(string proposed)
	{
		FileInfo? fileInfo = null;

		try
		{
//BUG: This should throw if invalid characters are used, but it does not...?!
			fileInfo = new FileInfo(proposed);
		}
		catch (ArgumentException)
		{}
		catch (PathTooLongException)
		{}
		catch (NotSupportedException)
		{}

		return ReferenceEquals(fileInfo, null);
	}

	private async Task<bool> TryWriteContentToFileAsync(string filename, string content, Encoding encoding, CancellationToken cancellationToken)
	{
		try
		{
			await File.WriteAllTextAsync(filename, content, encoding, cancellationToken).ConfigureAwait(false);
			Debug.WriteLine($"Wrote content to '{filename}'");
			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error writing to file {filename}");
			ConsoleHelper.PrintExceptionDetails(ex);
			return false;
		}
	}
}