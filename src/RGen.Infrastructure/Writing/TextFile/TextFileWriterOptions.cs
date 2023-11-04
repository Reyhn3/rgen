using System.IO;
using RGen.Domain.Writing;


namespace RGen.Infrastructure.Writing.TextFile;

public record struct TextFileWriterOptions(FileInfo FileName, string suffix) : IWriterOptions;