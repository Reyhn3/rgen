using RGen.Domain.Formatting;


namespace RGen.Infrastructure.Formatting.Console;

public record struct ConsoleFormatterOptions(bool IsColoringDisabled) : IFormatterOptions;