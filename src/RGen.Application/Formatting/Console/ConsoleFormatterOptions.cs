namespace RGen.Application.Formatting.Console;

public record struct ConsoleFormatterOptions(bool IsColoringDisabled) : IFormatterOptions;