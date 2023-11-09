using System;
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


public class TextFileWriter : IWriter
{
	private readonly ILogger<TextFileWriter> _logger;
	private readonly TextFileWriterOptions _options;

	public TextFileWriter(ILogger<TextFileWriter> logger, TextFileWriterOptions options)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_options = options;
	}

	public async Task<IResult> WriteAsync(RenderContext context, CancellationToken cancellationToken)
	{
		if (!TryGetOrCreateFileName(_options.FileName, out var filename))
			return Result.Failure(ResultCode.OutputFilePathError);

		if (!await TryWriteContentToFileAsync(filename!, context.Formatted, Encoding.UTF8, cancellationToken))
			return Result.Failure(ResultCode.OutputFileWriteError);

		_logger.LogInformation("Values exported to: {OutputFile}", filename);

		return Result.OK;
	}

	internal bool TryGetOrCreateFileName(FileInfo? optionsFileName, out string? filename)
	{
		if (optionsFileName == null)
			try
			{
				filename = GenerateRandomFileName();
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to create temporary file (no output filename was provided)");
				LogHelper.PrintExceptionDetails(ex);
				filename = null;
				return false;
			}

		try
		{
			var pathAndFilename = Path.GetFullPath(optionsFileName.ToString());
			var pathOnly = Path.GetDirectoryName(pathAndFilename);
			if (pathOnly != null)
				Directory.CreateDirectory(pathOnly);

			var isDirectoryOrFileOrNeither = IsDirectory(pathAndFilename);
			switch (isDirectoryOrFileOrNeither)
			{
				case true:
					_logger.LogDebug("No filename provided - generating random filename");
					filename = Path.Join(pathAndFilename, GenerateRandomFileName());
					return true;
				case false:
					_logger.LogDebug("Directory and filename provided");
					filename = pathAndFilename;
					return true;
				case null:
					_logger.LogDebug("No file or directory provided - generating temporary file");
					filename = string.IsNullOrWhiteSpace(Path.GetFileName(pathAndFilename)) ? GenerateRandomFileName() : pathAndFilename;
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
	}

	private static string GenerateRandomFileName() =>
		new(Path.GetTempFileName());

	internal static bool? IsDirectory(string path)
	{
		if (Directory.Exists(path))
			return true;

		if (File.Exists(path))
			return false;

		// Unknown: Neither directory nor file exists
		return null;
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
			_logger.LogError(ex, "Error writing to file {OutputFileName}", filename);
			LogHelper.PrintExceptionDetails(ex);
			return false;
		}
	}
}