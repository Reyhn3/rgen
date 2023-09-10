using RGen.Domain.Rendering;


namespace RGen.Infrastructure.Rendering.Console;

public record struct ConsoleRendererOptions(bool IsColoringDisabled) : IRendererOptions;