using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RGen.Domain;
using RGen.Domain.Rendering;
using RGen.Domain.Writing;
using RGen.Infrastructure.Logging;
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

	public async Task<IResult> WriteAsync(RenderContext context, CancellationToken cancellationToken)
	{
		if (!TryGetOrCreateFileName(_options.FileName, out var filename))
			return Result.Failure(ResultCode.OutputFilePathError);

		if (!await TryWriteContentToFileAsync(filename!, context.Raw, Encoding.UTF8, cancellationToken))
			return Result.Failure(ResultCode.OutputFileWriteError);

		_logger.LogInformation("Values exported to: {OutputFile}", filename);

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
			LogHelper.PrintExceptionDetails(ex);
			filename = null;
			return false;
		}

		try
		{
			var tempFileName = Path.GetTempFileName();
			filename = Path.ChangeExtension(tempFileName, "txt");
			_logger.LogDebug("Created temporary file {TemporaryFileName}", filename);

			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error constructing file path and name for output");
			LogHelper.PrintExceptionDetails(ex);
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
			_logger.LogDebug("Wrote content to {OutputFileName}", filename);
			return true;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Error writing to file {filename}");
			LogHelper.PrintExceptionDetails(ex);
			return false;
		}
	}
}