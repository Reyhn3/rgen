using RGen.Domain.Generating.Generators;


namespace RGen.Domain.Formatting;


public record struct IntegerFormatterOptions(IntegerBase Base) : IFormatterOptions;