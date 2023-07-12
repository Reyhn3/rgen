using System.IO;


namespace RGen.Application.Writing.TextFile;

public record struct PlainTextFileWriterOptions(FileInfo? FileName) : IWriterOptions;