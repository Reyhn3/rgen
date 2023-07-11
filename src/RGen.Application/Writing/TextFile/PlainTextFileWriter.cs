using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StdOut = System.Console;


namespace RGen.Application.Writing.TextFile;

public class PlainTextFileWriter : IWriter
{
	private readonly PlainTextFileWriterOptions _options;

	public PlainTextFileWriter(PlainTextFileWriterOptions options)
	{
		_options = options;
	}

	public async Task<ExitCode> WriteAsync(string values, CancellationToken cancellationToken)
	{
		if (!TryGetOrCreateFileName(_options.FileName, out var filename))
			return ExitCode.OutputFilePathError;

		if (!await TryWriteContentToFileAsync(filename!, values, Encoding.UTF8, cancellationToken))
			return ExitCode.OutputFileWriteError;

		StdOut.WriteLine();
		StdOut.WriteLine("Values exported to:");
		StdOut.ForegroundColor = ConsoleColor.White;
		StdOut.WriteLine(filename);
		StdOut.ResetColor();

		return ExitCode.OK;
	}

	private static bool TryGetOrCreateFileName(FileInfo? optionsFileName, out string? filename)
	{
		try
		{
			if (optionsFileName != null)
			{
//TEST: Relative path
//TEST: Path variables (like %TEMP%)
//TEST: Not a path (like invalid characters)
				filename = Path.GetFullPath(optionsFileName.ToString());

				var fullPath = Path.GetDirectoryName(filename);
				Directory.CreateDirectory(fullPath);

				return true;
			}
		}
		catch (Exception ex)
		{
			ConsoleHelper.PrintException(ex, "Specified file name and path is invalid");
			TraceHelper.PrintException(ex, "Error when verifying user specified path '{0}'", optionsFileName!);
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
			ConsoleHelper.PrintException(ex, "Error constructing file path and name for output");
			TraceHelper.PrintException(ex, "Error when creating temporary file");
			filename = null;
			return false;
		}
	}

	private static async Task<bool> TryWriteContentToFileAsync(string filename, string content, Encoding encoding, CancellationToken cancellationToken)
	{
		try
		{
			await File.WriteAllTextAsync(filename, content, encoding, cancellationToken).ConfigureAwait(false);
			Debug.WriteLine($"Wrote content to '{filename}'");
			return true;
		}
		catch (Exception ex)
		{
			ConsoleHelper.PrintException(ex, $"Error writing to file {filename}");
			TraceHelper.PrintException(ex, "Error writing to file {0}", filename);
			return false;
		}
	}
}