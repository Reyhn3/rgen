namespace RGen.Application.Formatting;

public record struct ConsoleFormatterOptions(bool IsColoringDisabled) : IFormatterOptions;