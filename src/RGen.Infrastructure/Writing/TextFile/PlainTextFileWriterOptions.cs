using System.IO;
using RGen.Domain.Writing;


namespace RGen.Infrastructure.Writing.TextFile;

public record struct PlainTextFileWriterOptions(FileInfo FileName) : IWriterOptions;